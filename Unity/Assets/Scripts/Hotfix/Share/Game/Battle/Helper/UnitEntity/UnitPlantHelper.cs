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
            int areaSize = 1000;
            int totalCellCount = 100;
            // int remainCount = 15;
            
            // TODO 去除最近地块只有一点点相邻的情况
            // int nearEdgeMinDistance = 5;   // 如果距离小于 不算相邻边

            Random r = world.RandomGenerator;
            var (centerPoints, borders) = BattleMapHelper.GenerateFortuneSites(areaSize, r, totalCellCount, 5);
            
            /*
            Dictionary<float2, Cell> pointCenter2Cells = new Dictionary<float2, Cell>();
            List<Cell> allCells = new List<Cell>();
            foreach (VEdge edge in edges)
            {
                var leftPoint = new float2((float)edge.Left.X, (float)edge.Left.Y);
                var rightPoint = new float2((float)edge.Right.X, (float)edge.Right.Y);
                
                var leftCell = pointCenter2Cells.GetValueOrDefault(leftPoint);
                if (leftCell == null)
                {
                    leftCell = new Cell();
                    leftCell.Center = leftPoint;
                    pointCenter2Cells[leftPoint] = leftCell;
                }
                
                leftCell.CellEdges.Add( new float4((float)edge.Start.X, (float)edge.Start.Y, (float)edge.End.X, (float)edge.End.Y));
                
                var rightCell = pointCenter2Cells.GetValueOrDefault(rightPoint);
                if (rightCell == null)
                {
                    rightCell = new Cell();
                    rightCell.Center = rightPoint;
                    pointCenter2Cells[rightPoint] = rightCell;
                }
                
                rightCell.CellEdges.Add( new float4((float)edge.Start.X, (float)edge.Start.Y, (float)edge.End.X, (float)edge.End.Y));
                
                
                /*if (Math.Sqrt((edge.Start.X - edge.End.X) * (edge.Start.X - edge.End.X) +
                        (edge.Start.Y - edge.End.Y) * (edge.Start.Y - edge.End.Y)) >= nearEdgeMinDistance)
                {
                    leftCell.NearCells.Add(rightPoint);
                    rightCell.NearCells.Add(leftPoint);
                }
                
                
                leftCell.NearCells.Add(rightPoint);
                rightCell.NearCells.Add(leftPoint);
                allCells.Add(leftCell);
                allCells.Add(rightCell);
            }
            
            // 检测
            if (pointSite.Count != pointCenter2Cells.Count)
            {
                Log.Warning($"生成地图异常 点和边关系不对应 {pointSite.Count} {pointCenter2Cells.Count} ");
            }
            
            //// 扣除地块逻辑
            // 找到距离中心比较近的地块
            Cell centerCell = null;
            float minDistance = float.MaxValue;
            float2 centerPoint = new float2((float)areaSize/ 2, (float)areaSize/2);
            foreach (var cell in allCells)
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

            List<Cell> generatedCells = new List<Cell>();
            generatedCells.Add(centerCell);
            for (int i = 0; i < remainCount; i++)
            {
                HashSet<float2> hadGeneratedCells = new HashSet<float2>();
                foreach (var generatedCell in generatedCells)
                {
                    hadGeneratedCells.Add(generatedCell.Center);
                }
                
                List<float2> allNextNearCells = new List<float2>();
                List<int> weights = new List<int>();
                foreach (var generatedCell in hadGeneratedCells)
                {
                    var cell = pointCenter2Cells.GetValueOrDefault(generatedCell);
                    foreach (var nearCell in cell.NearCells)
                    {
                        if (!hadGeneratedCells.Contains(nearCell) && !allNextNearCells.Contains(nearCell))
                        {
                            allNextNearCells.Add(nearCell);
                            
                            var nearCellData = pointCenter2Cells.GetValueOrDefault(nearCell);
                            // 统计已生成的相邻的地块
                            int nearCount = 0; 
                            foreach (var n in nearCellData.NearCells)
                            {
                                if (hadGeneratedCells.Contains(n))
                                {
                                    nearCount++;
                                }
                            }
                            
                            int weight = 10;
                            if (nearCount == 1)
                            {
                                weight = 100;
                            }else if (nearCount == 2)
                            {
                                weight = 30;
                            }
                            
                            weights.Add(weight);
                        }
                    }
                }

                if (allNextNearCells.Count == 0)
                {
                    break;
                }

                var randomIndex = BattleRandomHelper.RandomByWeight(world.RandomGenerator, weights);
                if (randomIndex < 0)
                {
                    break;
                }
                
                var nextNearPoint = allNextNearCells[randomIndex];
                var nextNearCell = pointCenter2Cells.GetValueOrDefault(nextNearPoint);
                generatedCells.Add(nextNearCell);
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
            foreach (var generatedCell in generatedCells)
            {
                foreach (var edge in generatedCell.CellEdges)
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
                    foreach (var generatedCell in generatedCells)
                    {
                        if (BattleMapHelper.IsPointInPolygon(new float2(centerPlantX, centerPlantY), generatedCell.CellEdges.ToList()))
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
            */

            var unitEntityPlaneCellGizmos = world.CreateEntity();
            var unitEntityCommonData1 = unitEntityPlaneCellGizmos.CreateUnitEntityElemData<UnitEntityCommonData>();
            unitEntityCommonData1.UnitEntityType = UnitEntityTypeEnum.GizmosDebug;
            var gizmosDebugInfo = unitEntityPlaneCellGizmos.CreateUnitEntityElemData<GizmosDebugInfo>();


            foreach (var centerPoint in centerPoints)
            {
                gizmosDebugInfo.CenterPoint.Add(new double2() { x = centerPoint.X, y = centerPoint.Y });
            }

            foreach (var border in borders)
            {
                gizmosDebugInfo.Borders.Add(new double4(border.Start.X, border.Start.Y, border.End.X, border.End.Y));
            }
            
            world.CreateEntityFinish(unitEntityPlaneCellGizmos);
            
            var unitEntityPlane = world.CreateEntity();
            var unitEntityCommonData = unitEntityPlane.CreateUnitEntityElemData<UnitEntityCommonData>();
            unitEntityCommonData.UnitEntityType = UnitEntityTypeEnum.Plane;

            var planeCellInfo = unitEntityPlane.CreateUnitEntityElemData<PlaneCellInfo>();
            planeCellInfo.Center = new float2(400, 400);
            planeCellInfo.MinX = 0;
            planeCellInfo.MinY = 0;
            
            planeCellInfo.MaxX = 1000;
            planeCellInfo.MaxY = 1000;
            world.CreateEntityFinish(unitEntityPlane);

            // 创建地块 UnitEntity
            /*
             foreach (var generatedCell in generatedCells)
            {
                var center = generatedCell.Center;
                var unitEntity = world.CreateEntity();

                var unitEntityCommonData = unitEntity.CreateUnitEntityElemData<UnitEntityCommonData>();
                unitEntityCommonData.UnitEntityType = UnitEntityTypeEnum.Plane;

                var planeCellInfo = unitEntity.CreateUnitEntityElemData<PlaneCellInfo>();
                planeCellInfo.Center = center;
                planeCellInfo.PlantEdges.AddRange(generatedCell.CellEdges);
                planeCellInfo.NearPlantCells.AddRange(generatedCell.NearCells);

                float minX = float.MaxValue;
                float minY = float.MaxValue;
                float maxX = float.MinValue;
                float maxY = float.MinValue;


                foreach (var edge in generatedCell.CellEdges)
                {
                    if (edge.x > maxX)
                    {
                        maxX = edge.x;
                    }

                    if (edge.x < minX)
                    {
                        minX = edge.x;
                    }

                    if (edge.y > maxY)
                    {
                        maxY = edge.y;
                    }

                    if (edge.y < minY)
                    {
                        minY = edge.y;
                    }

                    if (edge.z > maxX)
                    {
                        maxX = edge.z;
                    }

                    if (edge.z < minX)
                    {
                        minX = edge.z;
                    }

                    if (edge.w > maxY)
                    {
                        maxY = edge.w;
                    }

                    if (edge.w < minY)
                    {
                        minY = edge.w;
                    }
                }

                planeCellInfo.MinX = minX;
                planeCellInfo.MinY = minY;
                planeCellInfo.MaxX = maxX;
                planeCellInfo.MaxY = maxY;
                world.CreateEntityFinish(unitEntity);
            }*/
        }
    }
}