using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

// This Is Auto Generate, Do Not Edit!
namespace ET
{
    [Config]
    public partial class GlobalAttributeCategory : BaseCategory<GlobalAttributeCategory>
    {
        [BsonElement]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        private Dictionary<long, GlobalAttribute> dict = new();
		
        public override void Merge(object o)
        {
            GlobalAttributeCategory s = o as GlobalAttributeCategory;
            foreach (var kv in s.dict)
            {
                this.dict.Add(kv.Key, kv.Value);
            }
        }
		
        public GlobalAttribute GetById(long id)
        {
            return this.dict.GetValueOrDefault(id);
        }
		
        public bool Contain(long id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<long, GlobalAttribute> GetAll()
        {
            return this.dict;
        }

        public GlobalAttribute GetOne()
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

	public partial class GlobalAttribute: ProtoObject, IConfig
	{
		/// <summary>
		/// Id
		/// </summary>
		public long Id { get; set; }
		/// <summary>
		/// 属性编号
		/// </summary>
		public int AttrID { get; set; }
		/// <summary>
		/// 属性类型
		/// </summary>
		public string AttrType { get; set; }
		/// <summary>
		/// 属性值类型
		/// 1=数值
		/// 2=万分比数值
		/// </summary>
		public int ValueType { get; set; }

	}
}
