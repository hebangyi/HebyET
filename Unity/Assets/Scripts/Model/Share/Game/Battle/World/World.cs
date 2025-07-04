using System.Collections.Generic;

namespace ET
{
    public enum WorldMode : long
    {
        None = 0,
        Logic = 1,  // 纯逻辑 并且需要同步客户端
        Client = 2,  // 纯显示 从服务器接受消息
        All = 3,    // 所有
    }

    [ChildOf]
    public class World : Entity, IAwake<int>
    {
        public WorldMode WorldMode { get; set; } = WorldMode.None;
        // 当前世界逻辑帧
        public uint Frame = 1;
        // 世界状态
        public WorldStatusEnum WorldStatusEnum = WorldStatusEnum.Init;
        
        public Dictionary<long, UnitEntity> AllEntity = new ();
        //// 组件存储的数据
        // 玩家数据 PlayerId 2 Entity
        public Dictionary<long, UnitEntity> AllPlayers = new();
    }
}