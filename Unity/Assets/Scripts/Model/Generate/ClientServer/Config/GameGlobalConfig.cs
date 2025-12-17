using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

namespace ET
{
    [Config]
    public partial class GameGlobalConfigCategory : Singleton<GameGlobalConfigCategory>, IMerge
    {
        [BsonElement]
        public GameGlobalConfig Config;
        
        public void Merge(object o)
        {
            GameGlobalConfigCategory s = o as GameGlobalConfigCategory;
            this.Config = s.Config;
        }
    }

	public partial class GameGlobalConfig: ProtoObject, IKeyConfig
	{
		/// <summary></summary>
		public int TestKey1 { get; set; }
		/// <summary></summary>
		public string TestKey2 { get; set; }
		/// <summary></summary>
		public string TestKey3 { get; set; }

	}
}
