using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

namespace ET
{
    [Config]
    public partial class GlobalConfig1Category : Singleton<GlobalConfig1Category>, IMerge
    {
        [BsonElement]
        public GlobalConfig1 Config;
        
        public void Merge(object o)
        {
            GlobalConfig1Category s = o as GlobalConfig1Category;
            this.Config = s.Config;
        }
    }

	public partial class GlobalConfig1: ProtoObject, IKeyConfig
	{
		/// <summary></summary>
		public int TestKey1 { get; set; }
		/// <summary></summary>
		public string TestKey2 { get; set; }

	}
}
