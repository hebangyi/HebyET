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
        public static UnitEntity GeneratePlane(LogicWorld logicWorld, PlantGenContext plantGenContext)
        {
            // int generateCount = 30;
            // TODO 去除最近地块只有一点点相邻的情况
            // int nearEdgeMinDistance = 5;   // 如果距离小于 不算相邻边

            Random r = logicWorld.RandomGenerator;
            BattleMapHelper.GenerateBattleCells(plantGenContext);
            
            var unitEntity = logicWorld.CreateEntity();
            var commonData = unitEntity.CreateUnitEntityElemData<UnitEntityCommonData>();
            commonData.UnitEntityType = UETypeEnum.PlantMessage;
            
            var unitEntityMapMessage = unitEntity.CreateUnitEntityElemData<UnitEntityMapMessage>();
            unitEntityMapMessage.AreaSize = plantGenContext.InitData.AreaSize;
            
            PlantInfo plantInfo = PlantInfo.Create();
            foreach (var cell in plantGenContext.PlantData.GenCells)
            {
                var cellInfo = CellInfo.Create();
                cellInfo.CenterPoint = cell.Center;
                cellInfo.Borders.AddRange(cell.Borders);
                plantInfo.CellInfos.Add(cellInfo);
            }
            
            unitEntityMapMessage.PlantInfo = plantInfo;
            logicWorld.CreateEntityFinish(unitEntity);
            
            // 计算总的Cell数量
            var unitEntityPlaneCellGizmos = logicWorld.CreateEntity();
            var unitEntityCommonData1 = unitEntityPlaneCellGizmos.CreateUnitEntityElemData<UnitEntityCommonData>();
            unitEntityCommonData1.UnitEntityType = UETypeEnum.GizmosDebug;
            var gizmosDebugInfo = unitEntityPlaneCellGizmos.CreateUnitEntityElemData<GizmosDebugInfo>();
            
            foreach (var cell in plantGenContext.PlantData.GenCells)
            {
                gizmosDebugInfo.CenterPoints.Add(cell.Center);
            }

            foreach (var cell in plantGenContext.PlantData.GenCells)
            {
                gizmosDebugInfo.Borders.AddRange(cell.Borders);
            }

            gizmosDebugInfo.AreaSize = plantGenContext.InitData.AreaSize;
            logicWorld.CreateEntityFinish(unitEntityPlaneCellGizmos);
            return unitEntity;
        }
    }
}