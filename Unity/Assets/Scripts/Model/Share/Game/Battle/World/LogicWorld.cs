using System;
using System.Collections.Generic;
using Random = System.Random;

namespace ET
{

    [ChildOf]
    public partial class LogicWorld : World
    {
        public Random RandomGenerator;
        public Dictionary<long, UnitEntity> AllEntities = new ();
        
        // 当前世界逻辑帧
        public uint Frame = 1;
        // 下一次更新的时间
        public long NowMilliTime;
        public long NextUpdateMillTime;

        public float IntervalMillis
        {
            get
            {
                return GameConstant.LogicInterval * 1.0f / 1000;
            }
        }
        
        // 数据类型监听的EntityId
        public Dictionary<Type, HashSet<long>> DataTypeEntityIds = new ();
        
        

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
        public Dictionary<long, SyncDirtyUnitEntity> DirtyUnitEntities = new ();
    }
    
        
    // TODO 对象池
    public class SyncDirtyUnitEntity
    {
        public long InsId;
        // TODO 对象池
        public Dictionary<ushort, IUnitEntityElemData> DirtyElemDatas = new ();
    }
}