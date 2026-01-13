using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

// This Is Auto Generate, Do Not Edit!
namespace ET
{
    [Config]
    public partial class SkillConfigCategory : BaseCategory<SkillConfigCategory>
    {
        [BsonElement]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        private Dictionary<long, SkillConfig> dict = new();
		
        public override void Merge(object o)
        {
            SkillConfigCategory s = o as SkillConfigCategory;
            foreach (var kv in s.dict)
            {
                this.dict.Add(kv.Key, kv.Value);
            }
        }
		
        public SkillConfig GetById(long id)
        {
            return this.dict.GetValueOrDefault(id);
        }
		
        public bool Contain(long id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<long, SkillConfig> GetAll()
        {
            return this.dict;
        }

        public SkillConfig GetOne()
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

	public partial class SkillConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		public long Id { get; set; }
		/// <summary>技能Tag</summary>
		public PlayerSkillTagEnum PlayerSkillTag { get; set; }
		/// <summary>技能CD</summary>
		public int CD { get; set; }
		/// <summary></summary>
		public int[] TestArr { get; set; }

	}
}
