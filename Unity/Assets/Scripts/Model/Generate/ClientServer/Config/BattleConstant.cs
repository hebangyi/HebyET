using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

namespace ET
{
    [Config]
    public partial class BattleConstantCategory : Singleton<BattleConstantCategory>, IMerge
    {
        [BsonElement]
        public BattleConstant Config;
        
        public void Merge(object o)
        {
            BattleConstantCategory s = o as BattleConstantCategory;
            this.Config = s.Config;
        }
    }

	public partial class BattleConstant: ProtoObject, IKeyConfig
	{
		/// <summary></summary>
		public int TestKey1 { get; set; }
		/// <summary></summary>
		public string TestKey2 { get; set; }

	}
}
