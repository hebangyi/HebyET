using System;
using System.Numerics;
using Unity.Mathematics;

namespace ET
{
    public static class BattleHelper
    {
        public static long[] GetSearchBattleCellIds(UnitEntity unitEntity)
        {
            var aoiUnitEntity = unitEntity.GetComponent<AOIUnitEntity>();
            if (aoiUnitEntity == null)
            {
                return Array.Empty<long>();
            }
            
            var cellId = aoiUnitEntity.CellId;
            var (newX, newY) = AOIHelper.GetCellXY(cellId);
            var cellCount = (2 * GameConstant.AOIWatchCellRadius + 1) * (2 * GameConstant.AOIWatchCellRadius + 1);
            long[] cellIds = new long[cellCount];
            
            int index = 0;
            for (int x = newX - GameConstant.AOIWatchCellRadius; x <= newX + GameConstant.AOIWatchCellRadius; x++)
            {
                for (int y = newY - GameConstant.AOIWatchCellRadius; y <= newY + GameConstant.AOIWatchCellRadius; y++)
                {
                    cellIds[index] = AOIHelper.GetCellId(x, y);
                    index++;
                }
            }
            
            return cellIds;
        }
        
        public static float Distance(float2 from, float2 to)
        {
            return (float)Math.Sqrt((double)(from.x - to.x)*(from.x - to.x) + (double)(from.y - to.y)*(from.y - to.y));
        }

        public static float2 InnerCircleRandPoint(float radius)
        {
            var angle = RandomHelper.Random.NextDouble() * 2 * Math.PI;
            
            double x = Math.Cos(angle); // 余弦值
            double y = Math.Sin(angle); // 正弦值

            double r = RandomHelper.Random.NextDouble();
            x = x * r * radius;
            y = y * r * radius;
            return new float2((float)x, (float)y);
        }
        
    }
}

