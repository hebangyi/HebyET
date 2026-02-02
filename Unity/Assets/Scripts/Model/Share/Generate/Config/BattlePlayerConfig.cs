using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

// This Is Auto Generate, Do Not Edit!
namespace ET
{
    [Config]
    public partial class BattlePlayerConfigCategory : BaseCategory<BattlePlayerConfigCategory>
    {
        [BsonElement]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        private Dictionary<long, BattlePlayerConfig> dict = new();
		
        public override void Merge(object o)
        {
            BattlePlayerConfigCategory s = o as BattlePlayerConfigCategory;
            foreach (var kv in s.dict)
            {
                this.dict.Add(kv.Key, kv.Value);
            }
        }
		
        public BattlePlayerConfig GetById(long id)
        {
            return this.dict.GetValueOrDefault(id);
        }
		
        public bool Contain(long id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<long, BattlePlayerConfig> GetAll()
        {
            return this.dict;
        }

        public BattlePlayerConfig GetOne()
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

	public partial class BattlePlayerConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		public long Id { get; set; }
		/// <summary>Note说明</summary>
		public string Note { get; set; }
		/// <summary>资源</summary>
		public string Asset { get; set; }
		/// <summary>普攻技能</summary>
		public int AttackSkill { get; set; }

	}
}
