using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Random = System.Random;

namespace ET
{

    [ChildOf]
    public partial class LogicWorld : World
    {
        public Random RandomGenerator;
        
        public Dictionary<long, UnitEntity> AllEntities = new ();
        
        
        // 地图 UnitEntity
  
        
        // 当前世界逻辑帧
        public uint Frame = 1;
        // 下一次更新的时间
        public long NowMilliSeconds;
        public long NextUpdateMillTime;
        public int Interval = 100;

        public float IntervalMillis
        {
            get
            {
                return this.Interval * 1.0f / 1000;
            }
        }

        // 世界状态
        public WorldStatusEnum WorldStatusEnum = WorldStatusEnum.Init;
        
        ///// 
        // 逻辑端数据
        public UnitEntity PlantMessageUnitEntity;
        // GizmosDebug
        public UnitEntity GizmosDebugUnitEntity;
        
        
        // 玩家数据 PlayerId 2 Entity
        public Dictionary<long, UnitEntity> PlayerId2Players = new();
        
        public Dictionary<long, UnitEntity> Monsters = new Dictionary<long, UnitEntity>();
        
        // 逻辑帧脏数据
        public Dictionary<long, SyncDirtyUnitEntity> DirtyUnitEntities = new ();
    }
}