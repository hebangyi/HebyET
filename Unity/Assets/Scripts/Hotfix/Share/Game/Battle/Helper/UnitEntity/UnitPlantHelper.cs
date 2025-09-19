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
            var cells = BattleMapHelper.GenerateBattleCells(areaSize, r, pointCount, 5);
            
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