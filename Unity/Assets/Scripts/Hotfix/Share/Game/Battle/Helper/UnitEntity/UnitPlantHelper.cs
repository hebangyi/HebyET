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

        
        public static void GeneratePlane(LogicWorld logicWorld)
        {
            int areaSize = 2000;
            int pointCount = 200;
            // int generateCount = 30;
            
            // TODO 去除最近地块只有一点点相邻的情况
            // int nearEdgeMinDistance = 5;   // 如果距离小于 不算相邻边

            Random r = logicWorld.RandomGenerator;
            var cells = BattleMapHelper.GenerateMapCells(areaSize, r, pointCount, 5);
            
            //// 扣除地块逻辑
            // 找到距离中心比较近的地块
            CellData centerCellData = null;
            float minDistance = float.MaxValue;
            float2 centerPoint = new float2((float)areaSize/ 2, (float)areaSize/2);
            foreach (var cell in cells)
            {
                if (centerCellData == null)
                {
                    centerCellData = cell;
                    minDistance = (cell.Center.x - centerPoint.x) * (cell.Center.x - centerPoint.x) + (cell.Center.y - centerPoint.y) * (cell.Center.y - centerPoint.y);
                    continue;
                }
                
                var distance = (cell.Center.x - centerPoint.x) * (cell.Center.x - centerPoint.x) + (cell.Center.y - centerPoint.y) * (cell.Center.y - centerPoint.y);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    centerCellData = cell;
                }
            }
            
            
            var plantMessageUnitEntity = logicWorld.CreateEntity();
            var commonData = plantMessageUnitEntity.CreateUnitEntityElemData<UnitEntityCommonData>();
            commonData.UnitEntityType = UnitEntityTypeEnum.PlantMessage;
            
            var unitEntityMapMessage = plantMessageUnitEntity.CreateUnitEntityElemData<UnitEntityMapMessage>();
            unitEntityMapMessage.AreaSize = areaSize;


            PlantInfo plantInfo = PlantInfo.Create();
            foreach (var cell in cells)
            {
                var cellInfo = CellInfo.Create();
                cellInfo.CenterPoint = cell.Center;
                cellInfo.Borders.AddRange(cell.Borders);
                plantInfo.CellInfos.Add(cellInfo);
            }
            unitEntityMapMessage.PlantInfo = plantInfo;
            
            logicWorld.CreateEntityFinish(plantMessageUnitEntity);

            
            
            /*
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
            */
            
            // 计算总的Cell数量
            var unitEntityPlaneCellGizmos = logicWorld.CreateEntity();
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
            logicWorld.CreateEntityFinish(unitEntityPlaneCellGizmos);
            
        }
    }
}