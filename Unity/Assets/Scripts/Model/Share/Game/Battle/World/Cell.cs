using NativeCollection.UnsafeType;
using Unity.Mathematics;

namespace ET
{
    public class Cell
    {
        public float2 Center;
        public HashSet<float4> CellEdges = new ();
    }    
}


