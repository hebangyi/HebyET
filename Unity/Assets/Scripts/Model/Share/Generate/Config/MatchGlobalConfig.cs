using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

namespace ET
{
    // This Is Auto Generate , Do Not Edit!
    [Config]
    public partial class MatchGlobalConfigCategory : BaseCategory<MatchGlobalConfigCategory>
    {
        [BsonElement]
        public MatchGlobalConfig Config;
        
        public override void Merge(object o)
        {
            MatchGlobalConfigCategory s = o as MatchGlobalConfigCategory;
            this.Config = s.Config;
        }
        
        
        public override int Count()
        {
            if(Config == null)
            {
                return 0;
            }
            return 1;
        }
    }

	public partial class MatchGlobalConfig: ProtoObject, IKeyConfig
	{
		/// <summary></summary>
		public int MatchTimeOutSecond { get; set; }
		/// <summary></summary>
		public int TestMatchPlayerCount { get; set; }

	}
}
