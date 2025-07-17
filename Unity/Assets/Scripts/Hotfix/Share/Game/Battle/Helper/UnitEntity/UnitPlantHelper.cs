using System.Collections.Generic;
using Unity.Mathematics;
using VoronoiLib.Structures;

namespace ET
{
    public static class UnitPlantHelper
    {
        public static void GeneratePlant(World world, int seed)
        {
            var (pointSite, edges) = BattleMapHelper.GenerateFortuneSites(800, seed, 30);
            Dictionary<float2, Cell> pointCenter2Cells = new Dictionary<float2, Cell>();
            foreach (VEdge edge in edges)
            {
                var leftPoint = new float2((float)edge.Left.X, (float)edge.Left.Y);
                var rightPoint = new float2((float)edge.Right.X, (float)edge.Right.Y);
                
                var leftCell = pointCenter2Cells.GetValueOrDefault(leftPoint);
                if (leftCell == null)
                {
                    leftCell = new Cell();
                    leftCell.Center = leftPoint;
                    pointCenter2Cells[leftPoint] = leftCell;
                }
                
                leftCell.CellEdges.Add( new float4((float)edge.Left.X, (float)edge.Left.Y, (float)edge.Right.X, (float)edge.Right.Y));
                
                var rightCell = pointCenter2Cells.GetValueOrDefault(rightPoint);
                if (rightCell == null)
                {
                    rightCell = new Cell();
                    rightCell.Center = rightPoint;
                    pointCenter2Cells[rightPoint] = rightCell;
                }
                
                rightCell.CellEdges.Add( new float4((float)edge.Left.X, (float)edge.Left.Y, (float)edge.Right.X, (float)edge.Right.Y));
            }
            
            // 检测
            if (pointSite.Count != pointCenter2Cells.Count)
            {
                Log.Warning($"生成地图异常 点和边关系不对应 {pointSite.Count} {pointCenter2Cells.Count} 种子 : {seed}");
            }
            
            
            // 创建地块 UnitEntity
            foreach (var C2CellsKV in pointCenter2Cells)
            {
                var center = C2CellsKV.Key;
                var unitEntity = world.CreateEntity();
                var plantCellInfo = unitEntity.CreateUnitEntityElemData<PlantCellInfo>();
                plantCellInfo.Center = center;
                plantCellInfo.plantEdges.AddRange(C2CellsKV.Value.CellEdges);
                
                world.CreateEntityFinish(unitEntity);
            }
            
        }
    }
}