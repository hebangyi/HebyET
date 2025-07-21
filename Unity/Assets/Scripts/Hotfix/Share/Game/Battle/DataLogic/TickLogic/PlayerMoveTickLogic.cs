using System;
using System.Collections.Generic;
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
                
                // TODO? 计算加速度 让移动更加平滑 (饥荒好像没有计算加速度)
                var deltaX = Math.Cos(atan2) * unitEntityInfo.Speed;
                var deltaY = Math.Sin(atan2) * unitEntityInfo.Speed;
                
                float2 targetPoint = unitEntityPosition.Position + new float2((float)deltaX, (float)deltaY);

                var playerCellInfo = player.GetUnitEntityLogicElemData<UnitEntityPlayerCellInfo>();
                UnitEntity currentPlant = playerCellInfo.CurrentPlant;
                var planeCellInfo = currentPlant.GetUnitEntityElemData<PlaneCellInfo>();
                var plantEdges = planeCellInfo.PlantEdges;
                bool canMove = true;
                
                if (!BattleMapHelper.IsPointInPolygon(targetPoint, plantEdges))
                {
                    canMove = false;
                    // 1.是否在相邻的格子 如果在相邻的格子 切换到相邻的格子
                    var nearPlantCells = planeCellInfo.NearPlantCells;
                    bool findNear = false;
                    UnitEntity enterNearCellEntity = null;
                    
                    foreach (var nearPlantCell in nearPlantCells)
                    {
                        var n = world.Point2Plants.GetValueOrDefault(nearPlantCell);
                        if(n == null)
                            continue;
                        var nearPlaneCellInfo = n.GetUnitEntityElemData<PlaneCellInfo>();
                        
                        if (BattleMapHelper.IsPointInPolygon(targetPoint, nearPlaneCellInfo.PlantEdges))
                        {
                            findNear = true;
                            enterNearCellEntity = n;
                            break;
                        }
                    }

                    if (findNear)
                    {
                        playerCellInfo.CurrentPlant = enterNearCellEntity;
                        // 可以移动到相邻格子
                        canMove = true;
                    }
                }
                
                if (canMove)
                {
                    unitEntityPosition.Position = targetPoint;    
                }
            }
        }
    }
}