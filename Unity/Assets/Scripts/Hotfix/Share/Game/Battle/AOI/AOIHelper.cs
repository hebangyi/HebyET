using System;
using System.Collections.Generic;
using Unity.Mathematics;

namespace ET
{
    public static partial class AOIHelper
    {
        public static long GetCellId(int x, int y)
        {
            return ((long) x << 32) | (uint)y;
        }

        public static long GetCellId(float2 position)
        {
            int x = (int)position.x / GameConstant.AOICellSize;
            int y = (int)position.y / GameConstant.AOICellSize;
            return GetCellId(x, y);
        }

        public static (int, int) GetCellXY(float2 position)
        {
            int x = (int)position.x / GameConstant.AOICellSize;
            int y = (int)position.y / GameConstant.AOICellSize;
            return (x, y);
        }
        
        public static (int, int) GetCellXY(long cellId)
        {
            int x = (int)(cellId >> 32);
            int y = (int)cellId & 0xFFFF;
            return (x, y);
        }

        public static long[] GetAOICellIds(float2 position)
        {
            var (newX, newY) = GetCellXY(position);
            var cellCount = (2 * GameConstant.AOIWatchCellRadius + 1) * (2 * GameConstant.AOIWatchCellRadius + 1);
            long[] cellIds = new long[cellCount];
            
            int index = 0;
            for (int x = newX - GameConstant.AOIWatchCellRadius; x <= newX + GameConstant.AOIWatchCellRadius; x++)
            {
                for (int y = newY - GameConstant.AOIWatchCellRadius; y <= newY + GameConstant.AOIWatchCellRadius; y++)
                {
                    cellIds[index] = GetCellId(x, y);
                    index++;
                }
            }

            return cellIds;
        }
        

        
        
    }
}