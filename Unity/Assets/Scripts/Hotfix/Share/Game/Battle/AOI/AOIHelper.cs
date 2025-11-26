using System;
using System.Collections.Generic;

namespace ET
{
    public static partial class AOIHelper
    {
        public static long CreateCellId(long x, long y)
        {
            return (long) ((ulong) x << 32) | y;
        }

        public static Tuple<long, long> GetCellXY(long cellId)
        {
            long x = cellId >> 32;
            long y = cellId & 0xFFFF;
            return new Tuple<long, long>(x, y);
        }
        
        
    }
}