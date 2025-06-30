using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

namespace ET
{
    [Config]
    public partial class BattleSceneConfigCategory : Singleton<BattleSceneConfigCategory>, IMerge
    {
        [BsonElement]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        private Dictionary<int, BattleSceneConfig> dict = new();
		
        public void Merge(object o)
        {
            BattleSceneConfigCategory s = o as BattleSceneConfigCategory;
            foreach (var kv in s.dict)
            {
                this.dict.Add(kv.Key, kv.Value);
            }
        }
		
        public BattleSceneConfig GetById(int id)
        {
            return this.dict.GetValueOrDefault(id);
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, BattleSceneConfig> GetAll()
        {
            return this.dict;
        }

        public BattleSceneConfig GetOne()
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

	public partial class BattleSceneConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		public int Id { get; set; }
		/// <summary></summary>
		public string Name { get; set; }
		/// <summary>资源路径</summary>
		public string AssetPath { get; set; }

	}
}
