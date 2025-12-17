using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

namespace ET
{
    [Config]
    public partial class BattleGlobalConfigCategory : Singleton<BattleGlobalConfigCategory>, IMerge
    {
        [BsonElement]
        public BattleGlobalConfig Config;
        
        public void Merge(object o)
        {
            BattleGlobalConfigCategory s = o as BattleGlobalConfigCategory;
            this.Config = s.Config;
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
