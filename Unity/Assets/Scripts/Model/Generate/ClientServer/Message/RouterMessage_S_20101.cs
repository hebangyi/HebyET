using MemoryPack;
using System.Collections.Generic;

namespace ET
{
    [MemoryPackable]
    [Message(RouterMessage.SceneNodeInfo)]
    public partial class SceneNodeInfo : MessageObject
    {
        public static SceneNodeInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(SceneNodeInfo), isFromPool) as SceneNodeInfo;
        }

        /// <summary>
        /// SceneType
        /// </summary>
        [MemoryPackOrder(0)]
        public int SceneType { get; set; }

        [MemoryPackOrder(1)]
        public int ProcessId { get; set; }

        [MemoryPackOrder(2)]
        public int SceneId { get; set; }

        [MemoryPackOrder(3)]
        public string OuterIp { get; set; }

        [MemoryPackOrder(4)]
        public string InnerIp { get; set; }

        [MemoryPackOrder(5)]
        public int InnerPort { get; set; }

        [MemoryPackOrder(6)]
        public int OuterPort { get; set; }

        [MemoryPackOrder(7)]
        public int Status { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.SceneType = default;
            this.ProcessId = default;
            this.SceneId = default;
            this.OuterIp = default;
            this.InnerIp = default;
            this.InnerPort = default;
            this.OuterPort = default;
            this.Status = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    public static class RouterMessage
    {
        public const ushort SceneNodeInfo = 20102;
    }
}