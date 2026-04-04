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
        
        public override int Count()
        {
            return this.dict.Count;
        }
    }

	public partial class SkillConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		public long Id { get; set; }
		/// <summary>技能CD</summary>
		public int CD { get; set; }
		/// <summary>是否技能动画</summary>
		public int IsSkillAnimation { get; set; }
		/// <summary>技能动画开始时间</summary>
		public int SkillAnimationSTime { get; set; }
		/// <summary>技能动画标签</summary>
		public string SkillAniTag { get; set; }
		/// <summary>打断动画</summary>
		public string InterruptSkillAniTag { get; set; }
		/// <summary>Buff开始偏移释放时间</summary>
		public int BuffOffSetSTime { get; set; }
		/// <summary>buff执行栈</summary>
		public BuffStack[] BuffStacks { get; set; }

	}
}
