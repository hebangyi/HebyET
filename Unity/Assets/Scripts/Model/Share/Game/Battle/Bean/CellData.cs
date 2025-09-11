
using System.Collections.Generic;
using Unity.Mathematics;

namespace ET
{
    public class CellData
    {
        public float2 Center;
        public HashSet<float4> Borders = new ();
        
        public float plantMinX = 0;
        public float plantMinY = 0;
        public float plantMaxX = 0;
        public float plantMaxY = 0;
    }
}