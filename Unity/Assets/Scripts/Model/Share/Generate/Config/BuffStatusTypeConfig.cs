using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

// This Is Auto Generate, Do Not Edit!
namespace ET
{
    [Config]
    public partial class BuffStatusTypeConfigCategory : BaseCategory<BuffStatusTypeConfigCategory>
    {
        [BsonElement]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        private Dictionary<long, BuffStatusTypeConfig> dict = new();
		
        public override void Merge(object o)
        {
            BuffStatusTypeConfigCategory s = o as BuffStatusTypeConfigCategory;
            foreach (var kv in s.dict)
            {
                this.dict.Add(kv.Key, kv.Value);
            }
        }
		
        public BuffStatusTypeConfig GetById(long id)
        {
            return this.dict.GetValueOrDefault(id);
        }
		
        public bool Contain(long id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<long, BuffStatusTypeConfig> GetAll()
        {
            return this.dict;
        }

        public BuffStatusTypeConfig GetOne()
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

	public partial class BuffStatusTypeConfig: ProtoObject, IConfig
	{
		/// <summary>
		/// Id
		/// </summary>
		public long Id { get; set; }
		/// <summary>
		/// BuffStatus 状态类型
		/// </summary>
		public BuffStatus BuffStatus { get; set; }
		/// <summary>
		/// 是否叠加层数
		/// </summary>
		public bool IsBuffStackCount { get; set; }
		/// <summary>
		/// 如果能够叠加
		/// 最大叠加层数
		/// </summary>
		public int BuffStatusStatckMaxCount { get; set; }
		/// <summary>
		/// 叠加时间方式
		/// 0=最大叠加时间
		/// 1=最小叠加时间
		/// 2=叠加时间之和
		/// </summary>
		public int BuffStatusTimeAddType { get; set; }
		/// <summary>
		/// 可变Json参数
		/// </summary>
		public string BuffStatusParam { get; set; }

	}
}
