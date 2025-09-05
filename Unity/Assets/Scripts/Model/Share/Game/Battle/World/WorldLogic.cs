using System.Collections.Generic;
using Unity.Mathematics;

namespace ET
{
    public partial class World
    {
        //// 逻辑端数据
        // 玩家数据 PlayerId 2 Entity
        public Dictionary<long, UnitEntity> PlayerId2Players = new();
        // 中心坐标2地块
        public Dictionary<float2, UnitEntity> Point2Plants = new ();
        
        // 逻辑帧脏数据
        // TODO AOI机制
        public Dictionary<long, SyncDirtyUnitEntity> DirtyUnitEntities = new ();

    }
}

