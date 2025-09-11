using System.Collections.Generic;

namespace ET.Client
{
    [ChildOf]
    public class ClientWorld: Entity, IAwake
    {
        public Dictionary<long, UnitEntity> AllEntity = new ();
        
        // 当前世界逻辑帧
        public uint Frame = 1;
        
        //// 客户端显示数据
        public UnitEntity MyPlayer;
        public Dictionary<long, UnitEntity> ViewAllPlayers = new();
    }
}
