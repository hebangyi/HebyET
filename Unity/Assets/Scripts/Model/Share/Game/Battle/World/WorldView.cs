using System.Collections.Generic;

namespace ET
{
    public partial class World
    {
        //// 客户端显示数据
        public UnitEntity MyPlayer;
        public Dictionary<long, UnitEntity> ViewAllPlayers = new();
    } 
}
