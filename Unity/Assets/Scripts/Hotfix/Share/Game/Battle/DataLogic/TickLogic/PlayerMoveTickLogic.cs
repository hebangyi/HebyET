using System;
using System.Collections.Generic;
using Unity.Mathematics;

namespace ET
{
    [UnitEntityLogic]
    public class PlayerMoveTickLogic : IBattleLogicTick
    {
        public const float Rad2Deg = 57.29578f;
        
        public void OnTick(LogicWorld logicWorld)
        {
            var allPlayers = logicWorld.PlayerId2Players;
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
                
                // TODO? 计算加速度 让移动更加平滑 (饥荒好像没有计算加速度)
                var deltaX = Math.Cos(atan2) * unitEntityInfo.Speed;
                var deltaY = Math.Sin(atan2) * unitEntityInfo.Speed;
                
                float2 targetPoint = unitEntityPosition.Position + new float2((float)deltaX, (float)deltaY);
                unitEntityPosition.Position = targetPoint;
            }
        }
    }
}