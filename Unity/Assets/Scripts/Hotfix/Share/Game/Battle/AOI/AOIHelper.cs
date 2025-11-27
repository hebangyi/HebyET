using System;
using System.Collections.Generic;

namespace ET
{
    public static partial class AOIHelper
    {
        public static long GetCellId(int x, int y)
        {
            return ((long) x << 32) | (uint)y;
        }

        public static (int, int) GetCellXY(long cellId)
        {
            int x = (int)(cellId >> 32);
            int y = (int)cellId & 0xFFFF;
            return (x, y);
        }
        
        
    }
}