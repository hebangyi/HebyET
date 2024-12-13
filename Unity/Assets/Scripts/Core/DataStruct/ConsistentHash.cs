using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ET
{
    public class ConsistentHash<T>
    {
        private readonly SortedDictionary<int, T> _circle = new SortedDictionary<int, T>();
        private readonly int _replicas; // 每个节点的虚拟节点数量
        private readonly Func<string, int> _hashFunction;

        public ConsistentHash(int replicas = 100, Func<string, int> hashFunction = null)
        {
            _replicas = replicas;
            _hashFunction = hashFunction ?? DefaultHashFunction;
        }

        // 默认哈希函数：MD5取前4字节
        private static int DefaultHashFunction(string key)
        {
            using var md5 = MD5.Create();
            var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            return BitConverter.ToInt32(bytes, 0) & 0x7FFFFFFF; // 取非负数
        }

        // 添加节点
        public void Add(T node)
        {
            for (int i = 0; i < _replicas; i++)
            {
                var nodeKey = $"{node}:{i}";
                int hash = _hashFunction(nodeKey);
                _circle[hash] = node;
            }
        }

        // 移除节点
        public void Remove(T node)
        {
            for (int i = 0; i < _replicas; i++)
            {
                var nodeKey = $"{node}:{i}";
                int hash = _hashFunction(nodeKey);
                _circle.Remove(hash);
            }
        }

        // 根据键获取对应的节点
        public T Get(string key)
        {
            if (_circle.Count == 0)
                throw new InvalidOperationException("No nodes in the hash circle.");

            int hash = _hashFunction(key);
            if (!_circle.TryGetValue(hash, out var node))
            {
                // 顺时针找到最近的一个节点
                var tailMap = _circle.Keys;
                foreach (var k in tailMap)
                {
                    if (k >= hash)
                    {
                        node = _circle[k];
                        break;
                    }
                }

                // 如果没有找到，取第一个节点（环状结构）
                if (node == null)
                    node = _circle[_circle.Keys.Min()];
            }

            return node;
        }
    }
}