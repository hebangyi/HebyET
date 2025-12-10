using System;
using System.Numerics;
using Unity.Mathematics;

namespace ET
{
    public static class BattleHelper
    {
        public static float Distance(float2 from, float2 to)
        {
            return (float)Math.Sqrt((double)(from.x - to.x)*(from.x - to.x) + (double)(from.y - to.y)*(from.y - to.y));
        }
    }
}

