using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.Mathematics;
using VoronoiLib.Structures;
using Random = System.Random;

namespace ET
{
    public static class UnitPlaneHelper
    {
        public static void GeneratePlane(World world)
        {
            int areaSize = 2000;
            int totalCellCount = 200;
            // int generateCount = 30;
            
            // TODO 去除最近地块只有一点点相邻的情况
            // int nearEdgeMinDistance = 5;   // 如果距离小于 不算相邻边

            Random r = world.RandomGenerator;
            var cells = BattleMapHelper.GenerateMapCells(areaSize, r, totalCellCount, 5);
            
            //// 扣除地块逻辑
            // 找到距离中心比较近的地块
            Cell centerCell = null;
            float minDistance = float.MaxValue;
            float2 centerPoint = new float2((float)areaSize/ 2, (float)areaSize/2);
            foreach (var cell in cells)
            {
                if (centerCell == null)
                {
                    centerCell = cell;
                    minDistance = (cell.Center.x - centerPoint.x) * (cell.Center.x - centerPoint.x) + (cell.Center.y - centerPoint.y) * (cell.Center.y - centerPoint.y);
                    continue;
                }
                
                var distance = (cell.Center.x - centerPoint.x) * (cell.Center.x - centerPoint.x) + (cell.Center.y - centerPoint.y) * (cell.Center.y - centerPoint.y);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    centerCell = cell;
                }
            }
            
            
            var plantMessageUnitEntity = world.CreateEntity();
            var commonData = plantMessageUnitEntity.CreateUnitEntityElemData<UnitEntityCommonData>();
            commonData.UnitEntityType = UnitEntityTypeEnum.PlantMessage;
            
            var unitEntityMapMessage = plantMessageUnitEntity.CreateUnitEntityElemData<UnitEntityMapMessage>();
            int unitPlantSize = 1;
            float unitPlantRadius = unitPlantSize * 1.0f / 2;
            
            
            world.CreateEntityFinish(plantMessageUnitEntity);

            float plantMinX = float.MaxValue;
            float plantMinY = float.MaxValue;
            float plantMaxX = 0;
            float plantMaxY = 0;
            foreach (var cell in cells)
            {
                foreach (var edge in cell.Borders)
                {
                    if (edge.x > plantMaxX)
                    {
                        plantMaxX = edge.x;
                    }
                    
                    if (edge.x < plantMinX)
                    {
                        plantMinX = edge.x;
                    }
                    
                    if (edge.y > plantMaxY)
                    {
                        plantMaxY = edge.y;
                    }

                    if (edge.y < plantMinY)
                    {
                        plantMinY = edge.y;
                    }

                    if (edge.z > plantMaxX)
                    {
                        plantMaxX = edge.z;
                    }
                    
                    if (edge.z < plantMinX)
                    {
                        plantMinX = edge.z;
                    }
                    
                    if (edge.w > plantMaxY)
                    {
                        plantMaxY = edge.w;
                    }
                    
                    if (edge.w < plantMinY)
                    {
                        plantMinY = edge.w;
                    }
                }
            }
            
            // 计算总的Cell数量
            int xPlantCount = (int)(plantMaxX / unitPlantSize) + 1;
            int yPlantCount = (int)(plantMaxY / unitPlantSize) + 1;
            int totalPlantCount = xPlantCount * yPlantCount;
            unitEntityMapMessage.MapData = new bool[totalPlantCount].ToList();
            unitEntityMapMessage.MapWidth = xPlantCount;
            unitEntityMapMessage.MapHeight = yPlantCount;
            unitEntityMapMessage.UnitCellSize = unitPlantSize;

            for (int x = 0; x < xPlantCount; x++)
            {
                for (int y = 0; y < yPlantCount; y++)
                {
                    // 判断中心点是否在Cell
                    float centerPlantX = x * unitPlantSize + unitPlantRadius;
                    float centerPlantY = y * unitPlantSize + unitPlantRadius;

                    bool isPoint = false;
                    foreach (var cell in cells)
                    {
                        if (BattleMapHelper.IsPointInPolygon(new float2(centerPlantX, centerPlantY), cell.Borders.ToList()))
                        {
                            isPoint = true;
                            break;
                        }
                    }

                    if (isPoint)
                    {
                        var target = x * xPlantCount + y;
                        unitEntityMapMessage.MapData[target] = true;
                    }
                }
            }
            

            var unitEntityPlaneCellGizmos = world.CreateEntity();
            var unitEntityCommonData1 = unitEntityPlaneCellGizmos.CreateUnitEntityElemData<UnitEntityCommonData>();
            unitEntityCommonData1.UnitEntityType = UnitEntityTypeEnum.GizmosDebug;
            var gizmosDebugInfo = unitEntityPlaneCellGizmos.CreateUnitEntityElemData<GizmosDebugInfo>();
            
            foreach (var cell in cells)
            {
                gizmosDebugInfo.CenterPoints.Add(cell.Center);
            }

            foreach (var cell in cells)
            {
                gizmosDebugInfo.Borders.AddRange(cell.Borders);
            }

            gizmosDebugInfo.AreaSize = areaSize;
            world.CreateEntityFinish(unitEntityPlaneCellGizmos);
            
        }
    }
}