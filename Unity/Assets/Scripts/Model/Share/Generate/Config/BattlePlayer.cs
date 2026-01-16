using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

// This Is Auto Generate, Do Not Edit!
namespace ET
{
    [Config]
    public partial class BattlePlayerCategory : BaseCategory<BattlePlayerCategory>
    {
        [BsonElement]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        private Dictionary<long, BattlePlayer> dict = new();
		
        public override void Merge(object o)
        {
            BattlePlayerCategory s = o as BattlePlayerCategory;
            foreach (var kv in s.dict)
            {
                this.dict.Add(kv.Key, kv.Value);
            }
        }
		
        public BattlePlayer GetById(long id)
        {
            return this.dict.GetValueOrDefault(id);
        }
		
        public bool Contain(long id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<long, BattlePlayer> GetAll()
        {
            return this.dict;
        }

        public BattlePlayer GetOne()
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

	public partial class BattlePlayer: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		public long Id { get; set; }
		/// <summary>普攻技能</summary>
		public long[] SkillIds { get; set; }

	}
}
