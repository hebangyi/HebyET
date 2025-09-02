using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

namespace ET
{
    [Config]
    public partial class GlobalConfigCategory : Singleton<GlobalConfigCategory>, IMerge
    {
        [BsonElement]
        public GlobalConfig Config;
        
        public void Merge(object o)
        {
            GlobalConfigCategory s = o as GlobalConfigCategory;
            this.Config = s.Config;
        }
    }

	public partial class GlobalConfig: ProtoObject, IKeyConfig
	{
		/// <summary></summary>
		public int TestKey1 { get; set; }
		/// <summary></summary>
		public string TestKey2 { get; set; }

	}
}
