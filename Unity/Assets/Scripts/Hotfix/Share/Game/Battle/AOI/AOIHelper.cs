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
            return GetCellId((int)position.x, (int)position.y);
        }

        public static (int, int) GetCellXY(long cellId)
        {
            int x = (int)(cellId >> 32);
            int y = (int)cellId & 0xFFFF;
            return (x, y);
        }
        
        
    }
}