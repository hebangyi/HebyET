
using System.Collections.Generic;
using Unity.Mathematics;

namespace ET
{
    public class CellData
    {
        public float2 Center;
        public HashSet<float4> Borders = new ();
        public HashSet<CellData> NearCellDataSet = new ();
    }
}