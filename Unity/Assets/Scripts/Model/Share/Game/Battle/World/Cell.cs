
using System.Collections.Generic;
using Unity.Mathematics;

namespace ET
{
    public class Cell
    {
        public float2 Center;
        public HashSet<float4> CellEdges = new ();
        public HashSet<float2> NearCells = new ();
    }    
}


