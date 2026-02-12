using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

// This Is Auto Generate, Do Not Edit!
namespace ET
{
    [Config]
    public partial class BuffConfigCategory : BaseCategory<BuffConfigCategory>
    {
        [BsonElement]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        private Dictionary<long, BuffConfig> dict = new();
		
        public override void Merge(object o)
        {
            BuffConfigCategory s = o as BuffConfigCategory;
            foreach (var kv in s.dict)
            {
                this.dict.Add(kv.Key, kv.Value);
            }
        }
		
        public BuffConfig GetById(long id)
        {
            return this.dict.GetValueOrDefault(id);
        }
		
        public bool Contain(long id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<long, BuffConfig> GetAll()
        {
            return this.dict;
        }

        public BuffConfig GetOne()
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

	public partial class BuffConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		public long Id { get; set; }
		/// <summary>buff执行类型</summary>
		public BuffTypeEnum BuffType { get; set; }
		/// <summary>Buff持续时间(毫秒)</summary>
		public int DurationTime { get; set; }
		/// <summary>可变Json参数</summary>
		public string BuffParam { get; set; }

	}
}
