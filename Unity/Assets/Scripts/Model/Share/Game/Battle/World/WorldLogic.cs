using System.Collections.Generic;

namespace ET
{
    public partial class World
    {
        //// 逻辑端数据
        // 玩家数据 PlayerId 2 Entity
        public Dictionary<long, UnitEntity> AllPlayers = new();
        // 逻辑帧脏数据
        // TODO AOI机制
        public Dictionary<long, SyncDirtyUnitEntity> DirtyUnitEntities = new ();

    }
}

