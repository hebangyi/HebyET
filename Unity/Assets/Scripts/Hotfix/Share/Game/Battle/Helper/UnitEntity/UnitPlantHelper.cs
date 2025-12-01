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
            BattleMapHelper.GenerateBattleCells(plantGenContext);
            
            var unitEntity = logicWorld.CreateEntity();
            var playerInitContext = unitEntity.AddComponent<PlaneInitContext>();
            playerInitContext.PlantGenContext = plantGenContext;
            logicWorld.CreateEntityFinish(unitEntity, UETypeEnum.PlantMessage);
            
            var unitEntityPlaneCellGizmos = logicWorld.CreateEntity();
            playerInitContext = unitEntityPlaneCellGizmos.AddComponent<PlaneInitContext>();
            playerInitContext.PlantGenContext = plantGenContext;
            
            logicWorld.CreateEntityFinish(unitEntityPlaneCellGizmos, UETypeEnum.GizmosDebug);
            return unitEntity;
        }
    }
}