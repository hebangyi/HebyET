using System.Collections.Generic;

namespace ET
{
    public enum WorldMode : long
    {
        None = 0,
        Server = 1,  // 纯逻辑 并且需要同步客户端
        Client = 2,  // 纯显示 从服务器接受消息
    }

    [ChildOf]
    public class World : Entity, IAwake
    {
        public WorldMode Mode{get; set; }
        
        public Dictionary<long, UnitEntity> AllEntity = new ();
        //// 组件存储的数据
        // 玩家数据 PlayerId 2 Entity
        public Dictionary<long, UnitEntity> AllPlayers = new();
    }
}