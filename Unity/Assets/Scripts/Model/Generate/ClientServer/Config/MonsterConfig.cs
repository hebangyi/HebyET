using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

// This Is Auto Generate, Do Not Edit!
namespace ET
{
    [Config]
    public partial class MonsterConfigCategory : Singleton<MonsterConfigCategory>, IMerge
    {
        [BsonElement]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        private Dictionary<long, MonsterConfig> dict = new();
		
        public void Merge(object o)
        {
            MonsterConfigCategory s = o as MonsterConfigCategory;
            foreach (var kv in s.dict)
            {
                this.dict.Add(kv.Key, kv.Value);
            }
        }
		
        public MonsterConfig GetById(long id)
        {
            return this.dict.GetValueOrDefault(id);
        }
		
        public bool Contain(long id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<long, MonsterConfig> GetAll()
        {
            return this.dict;
        }

        public MonsterConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            
            var enumerator = this.dict.Values.GetEnumerator();
            enumerator.MoveNext();
            return enumerator.Current; 
        }
    }

	public partial class MonsterConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		public long Id { get; set; }
		/// <summary>Note说明</summary>
		public string Note { get; set; }
		/// <summary>名称</summary>
		public string Name { get; set; }
		/// <summary>Asset资源名称</summary>
		public string AssetName { get; set; }

	}
}
