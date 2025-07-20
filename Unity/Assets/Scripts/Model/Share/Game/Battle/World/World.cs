using System;
using System.Collections.Generic;
using System.Linq;

namespace ET
{
    public enum WorldMode : long
    {
        None = 0,
        Logic = 1,  // 纯逻辑 并且需要同步客户端
        View = 2,  // 纯显示 从服务器接受消息
        // All = 3,    // 所有
    }

    [ChildOf]
    public partial class World : Entity, IAwake<int>
    {
        public IDirtyHandler DirtyHandler;
        public ISyncHandler SyncHandler;
        public Random RandomGenerator;
        
        public WorldMode WorldMode { get; set; } = WorldMode.None;
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