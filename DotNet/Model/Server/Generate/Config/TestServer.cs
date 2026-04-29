using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

// This Is Auto Generate, Do Not Edit!
namespace ET
{
    [Config]
    public partial class TestServerCategory : BaseCategory<TestServerCategory>
    {
        [BsonElement]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        private Dictionary<long, TestServer> dict = new();
		
        public override void Merge(object o)
        {
            TestServerCategory s = o as TestServerCategory;
            foreach (var kv in s.dict)
            {
                this.dict.Add(kv.Key, kv.Value);
            }
        }
		
        public TestServer GetById(long id)
        {
            return this.dict.GetValueOrDefault(id);
        }
		
        public bool Contain(long id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<long, TestServer> GetAll()
        {
            return this.dict;
        }

        public TestServer GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            
            var enumerator = this.dict.Values.GetEnumerator();
            enumerator.MoveNext();
            return enumerator.Current; 
        }
        
        public override int Count()
        {
            return this.dict.Count;
        }
    }

	public partial class TestServer: ProtoObject, IConfig
	{
		/// <summary>
		/// Id
		/// </summary>
		public long Id { get; set; }
		/// <summary>
		/// 章节名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 地图类型
		/// </summary>
		public int MapSceneType { get; set; }
		/// <summary>
		/// 资源路径
		/// </summary>
		public string AssetPath { get; set; }

	}
}
