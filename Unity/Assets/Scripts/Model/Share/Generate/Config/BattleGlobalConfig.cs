using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

namespace ET
{
    // This Is Auto Generate , Do Not Edit!
    [Config]
    public partial class BattleGlobalConfigCategory : BaseCategory<BattleGlobalConfigCategory>
    {
        [BsonElement]
        public BattleGlobalConfig Config;
        
        public override void Merge(object o)
        {
            BattleGlobalConfigCategory s = o as BattleGlobalConfigCategory;
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

	public partial class BattleGlobalConfig: ProtoObject, IKeyConfig
	{
		/// <summary>关联BattleMapConfig表-1</summary>
		public long TestMapConfigId { get; set; }
		/// <summary>Tile的边长</summary>
		public int TileMapUnitSize { get; set; }

	}
}
