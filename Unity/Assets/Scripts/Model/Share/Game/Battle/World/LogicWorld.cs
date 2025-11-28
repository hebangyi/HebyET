using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Random = System.Random;

namespace ET
{

    [ChildOf]
    public partial class LogicWorld : World
    {
        // 数据同步 
        public IDirtyHandler DirtyHandler;
        public ISyncHandler SyncHandler;
        public Random RandomGenerator;
        
        public Dictionary<long, UnitEntity> AllEntities = new ();
        // 地图 UnitEntity
  
        
        // 当前世界逻辑帧
        public uint Frame = 1;
        // 下一次更新的时间
        public long NextUpdateMillTime;
        public int Interval = 100;
        // 世界状态
        public WorldStatusEnum WorldStatusEnum = WorldStatusEnum.Init;
        
        ///// 
        // 逻辑端数据
        public UnitEntity PlantMessageUnitEntity;
        // GizmosDebug
        public UnitEntity GizmosDebugUnitEntity;
        
        
        // 玩家数据 PlayerId 2 Entity
        public Dictionary<long, UnitEntity> PlayerId2Players = new();
        
        
        
        // 逻辑帧脏数据
        // TODO AOI机制
        public Dictionary<long, SyncDirtyUnitEntity> DirtyUnitEntities = new ();
    }
}