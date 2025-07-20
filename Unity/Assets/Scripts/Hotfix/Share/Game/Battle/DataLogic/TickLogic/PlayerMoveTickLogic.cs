using System;
using Unity.Mathematics;

namespace ET
{
    [UnitEntityLogic]
    public class PlayerMoveTickLogic : IUnitEntityTickLogic
    {
        public const float Rad2Deg = 57.29578f;
        
        public void OnTick(World world)
        {
            var allPlayers = world.PlayerId2Players;
            foreach (var playerKv in allPlayers)
            {
                var player = playerKv.Value;
                var unitEntityPlayerOperation = player.GetUnitEntityLogicElemData<UnitEntityPlayerOperation>();
                if (unitEntityPlayerOperation.MoveAngel == -1000)
                {
                    continue;
                }

                var unitEntityPosition = player.GetUnitEntityElemData<UnitEntityPosition>();
                var unitEntityInfo = player.GetUnitEntityElemData<UnitEntityInfo>();
                var atan2 = unitEntityPlayerOperation.MoveAngel / Rad2Deg; 
                var deltaX = Math.Cos(atan2) * unitEntityInfo.Speed;
                var deltaZ = Math.Sin(atan2) * unitEntityInfo.Speed;
                
                
                // 记录脏数据
                unitEntityPosition.Position += new float3((float)deltaX, 0, (float)deltaZ);
            }
        }
    }
}