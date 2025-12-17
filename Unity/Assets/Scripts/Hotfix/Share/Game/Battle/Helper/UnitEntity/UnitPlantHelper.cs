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
    
            
            logicWorld.Create(UETypeEnum.GizmosDebug, plantGenContext);
            return logicWorld.Create(UETypeEnum.PlantMessage, plantGenContext);
        }
    }
}