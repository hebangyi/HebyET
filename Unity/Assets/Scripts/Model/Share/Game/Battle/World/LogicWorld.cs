using System;
using System.Collections.Generic;
using System.Linq;

namespace ET
{

    [ChildOf]
    public partial class World : Entity, IAwake
    {
        public IDirtyHandler DirtyHandler;
        public ISyncHandler SyncHandler;
        public Random RandomGenerator;
        
        // 当前世界逻辑帧
        public uint Frame = 1;
        // 下一次更新的时间
        public long NextUpdateMillTime;

        public int Interval = 100;
        // 世界状态
        public WorldStatusEnum WorldStatusEnum = WorldStatusEnum.Init;
        
        public Dictionary<long, UnitEntity> AllEntity = new ();
    }
}