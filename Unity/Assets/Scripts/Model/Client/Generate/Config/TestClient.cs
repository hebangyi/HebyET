using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

// This Is Auto Generate, Do Not Edit!
namespace ET
{
    [Config]
    public partial class TestClientCategory : BaseCategory<TestClientCategory>
    {
        [BsonElement]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        private Dictionary<long, TestClient> dict = new();
		
        public override void Merge(object o)
        {
            TestClientCategory s = o as TestClientCategory;
            foreach (var kv in s.dict)
            {
                this.dict.Add(kv.Key, kv.Value);
            }
        }
		
        public TestClient GetById(long id)
        {
            return this.dict.GetValueOrDefault(id);
        }
		
        public bool Contain(long id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<long, TestClient> GetAll()
        {
            return this.dict;
        }

        public TestClient GetOne()
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

	public partial class TestClient: ProtoObject, IConfig
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
