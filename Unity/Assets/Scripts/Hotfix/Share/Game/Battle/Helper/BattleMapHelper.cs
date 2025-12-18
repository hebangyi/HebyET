using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.Mathematics;
using VoronoiLib;
using VoronoiLib.Structures;
using Random = System.Random;

namespace ET
{
    public static class BattleMapHelper
    {
        // 判断点是否在多边形内部
        public static bool IsPointInPolygon(float2 targetPoint, List<float4> polygon)
        {
            if (polygon == null || polygon.Count < 3)
                return false;

            /*if (!IsSimpleFilter(targetPoint, polygon))
                return false;*/

            bool inside = false;
            int n = polygon.Count;
            float tx = targetPoint.x;
            float ty = targetPoint.y;

            // 遍历所有边
            foreach (var p in polygon)
            {
                float xi = p.x;
                float yi = p.y;
                float xj = p.z;
                float yj = p.w;

                float2 start = new float2(xi, yi);
                float2 end = new float2(xj, yj);

                // 检查点是否在多边形的边上
                if (IsPointOnEdge(targetPoint, start, end))
                    return true;

                // 射线法核心判断
                if (((yi > ty) != (yj > ty)) &&
                    (tx < (xj - xi) * (ty - yi) / (yj - yi) + xi))
                {
                    inside = !inside;
                }
            }

            return inside;
        }

        private static bool IsSimpleFilter(float2 targetPoint, List<float4> polygon)
        {
            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            foreach (var edge in polygon)
            {
                if (edge.x > maxX)
                {
                    maxX = edge.x;
                }

                if (edge.x < minX)
                {
                    minX = edge.x;
                }

                if (edge.y > maxY)
                {
                    maxY = edge.y;
                }

                if (edge.y < minY)
                {
                    minY = edge.y;
                }

                if (edge.z > maxX)
                {
                    maxX = edge.z;
                }

                if (edge.z < minX)
                {
                    minX = edge.z;
                }

                if (edge.w > maxY)
                {
                    maxY = edge.w;
                }

                if (edge.w < minY)
                {
                    minY = edge.w;
                }
            }

            if (targetPoint.x < minX || targetPoint.x > maxX || targetPoint.y < minY || targetPoint.y > maxY)
            {
                return false;
            }

            return true;
        }

        private static bool IsPointOnEdge(float2 p, float2 start, float2 end)
        {
            const double epsilon = 1e-10;

            // 检查点是否在矩形边界内
            if (p.x < Math.Min(start.x, end.x) - epsilon ||
                p.x > Math.Max(start.x, end.x) + epsilon ||
                p.y < Math.Min(start.y, end.y) - epsilon ||
                p.y > Math.Max(start.y, end.y) + epsilon)
                return false;

            // 检查点是否在直线上（叉积接近0）
            double crossProduct = (p.y - start.y) * (end.x - start.x) - (p.x - start.x) * (end.y - start.y);
            return Math.Abs(crossProduct) < epsilon;
        }

        public static void GenerateBattleCells(PlantGenContext plantGenContext)
        {
            // 所有的cells
            GenerateMapAllCells(plantGenContext);
            GenerateRealCells(plantGenContext);
            GenerateCellReSize(plantGenContext);
        }
        
        public static void GenerateMapAllCells(PlantGenContext plantGenContext)
        {
            var initData = plantGenContext.InitData;
            var plantData = plantGenContext.PlantData;
            var areaSize = initData.AreaSize;
            var (centerPoints, borders) = GenerateFortuneSites(initData.AreaSize, initData.Random, initData.CellPointCount, initData.CellPointMinDistance);
            Dictionary<float2, CellData> pointCenter2Cells = new Dictionary<float2, CellData>();
            Dictionary<int, CellData> allCellsDict = new Dictionary<int, CellData>();
            int idGen = 0;
            foreach (VEdge edge in borders)
            {
                var leftPoint = new float2((float)edge.Left.X, (float)edge.Left.Y);
                var rightPoint = new float2((float)edge.Right.X, (float)edge.Right.Y);

                var leftCell = pointCenter2Cells.GetValueOrDefault(leftPoint);
                if (leftCell == null)
                {
                    leftCell = new CellData();
                    leftCell.Id = ++idGen;
                    leftCell.Center = leftPoint;
                    pointCenter2Cells[leftPoint] = leftCell;
                }

                leftCell.Borders.Add(new float4((float)edge.Start.X, (float)edge.Start.Y, (float)edge.End.X, (float)edge.End.Y));

                var rightCell = pointCenter2Cells.GetValueOrDefault(rightPoint);
                if (rightCell == null)
                {
                    rightCell = new CellData();
                    rightCell.Id = ++idGen;
                    rightCell.Center = rightPoint;
                    pointCenter2Cells[rightPoint] = rightCell;
                }

                rightCell.Borders.Add(new float4((float)edge.Start.X, (float)edge.Start.Y, (float)edge.End.X, (float)edge.End.Y));

                leftCell.NearCellDataSet.Add(rightCell);
                rightCell.NearCellDataSet.Add(leftCell);
            }

            // 检查边的闭合性
            foreach (var pointCell in pointCenter2Cells.Values)
            {
                var cellBorders = pointCell.Borders.ToList();
                bool isRight = true;
                for (int i = 0; i < cellBorders.Count; i++)
                {
                    var x1 = cellBorders[i].x;
                    var y1 = cellBorders[i].y;
                    var x2 = cellBorders[i].z;
                    var y2 = cellBorders[i].w;

                    if (x1 == 0 || x2 == 0 || y1 == 0 || y2 == 0
                        || x1 == areaSize || x2 == areaSize || y1 == areaSize || y2 == areaSize)
                    {
                        isRight = false;
                        break;
                    }
                }

                if (isRight)
                {
                    allCellsDict.Add(pointCell.Id, pointCell);
                }
            }
            
            // 临边数据更新
            foreach (var cellData in allCellsDict.Values)
            {
                foreach (var nearCellData in cellData.NearCellDataSet.ToList())
                {
                    if (!allCellsDict.ContainsKey(nearCellData.Id))
                    {
                        cellData.NearCellDataSet.Remove(nearCellData);
                    }
                }
            }
            

            plantData.AllCells = allCellsDict.Values.ToList();
        }
        
        public static void GenerateRealCells(PlantGenContext plantGenContext)
        {
            var initData = plantGenContext.InitData;
            var plantData = plantGenContext.PlantData;
            var areaSize = initData.AreaSize;
            var cellDataList = plantData.AllCells;
            var genCells = plantData.RealCells;
         
            Dictionary<int, GenCellData> allGenCellDataDict = new ();
            HashSet<int> generateCellIds = new HashSet<int>();
            
            // 生成所有的GenCellData
            foreach (var cellData in cellDataList)
            {
                GenCellData genCellData = new ();
                genCellData.Id = cellData.Id;
                genCellData.IsInMap = false;
                genCellData.CellData = cellData;

                foreach (var nearCellData in cellData.NearCellDataSet)
                {
                    genCellData.NearCellIds.Add(nearCellData.Id);
                }
                
                allGenCellDataDict[cellData.Id] = genCellData;
            }
            
            // 找到距离中心比较近的地块
            CellData centerCellData = null;
            float minDistance = float.MaxValue;
            float2 centerPoint = new float2((float)areaSize/ 2, (float)areaSize/2);
            foreach (var cell in cellDataList)
            {
                if (centerCellData == null)
                {
                    centerCellData = cell;
                    minDistance = (cell.Center.x - centerPoint.x) * (cell.Center.x - centerPoint.x) + (cell.Center.y - centerPoint.y) * (cell.Center.y - centerPoint.y);
                    continue;
                }
                
                var distance = (cell.Center.x - centerPoint.x) * (cell.Center.x - centerPoint.x) + (cell.Center.y - centerPoint.y) * (cell.Center.y - centerPoint.y);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    centerCellData = cell;
                }
            }

            var centerGenCellData = allGenCellDataDict.GetValueOrDefault(centerCellData.Id);
            centerGenCellData.IsInMap = true;
            generateCellIds.Add(centerCellData.Id);
            
            // 在总地块中生成地块
            Dictionary<int, GenCellData> nextGenCellDataDict = new ();
            List<int> weightList = new List<int>();
            for (int i = 0; i < initData.GenCellCount -1; i++)
            {
                nextGenCellDataDict.Clear();
                weightList.Clear();
                
                foreach (var genCellId in generateCellIds)
                {
                    var genCellData = allGenCellDataDict.GetValueOrDefault(genCellId);
                    foreach (var nearCellId in genCellData.NearCellIds)
                    {
                        if (generateCellIds.Contains(nearCellId))
                        {
                            continue;
                        }
                        
                        var nearCellData = allGenCellDataDict.GetValueOrDefault(nearCellId);
                        if (nearCellData.IsInMap)
                        {
                            continue;
                        }
                        
                        nextGenCellDataDict[nearCellId] = nearCellData;
                    }
                }
                
                var nextCells = nextGenCellDataDict.Values.ToList();
                
                for(int z = 0; z < nextCells.Count; z++)
                {
                    var nextGenCellData = nextCells[z];
                    int inMapCellCount = 0;

                    foreach (var nearCellId in nextGenCellData.NearCellIds)
                    {
                        var nearCellData = allGenCellDataDict.GetValueOrDefault(nearCellId);
                        if(nearCellData.IsInMap)
                        {
                            inMapCellCount++;
                        }
                    }

                    int weight = 0;
                    if (inMapCellCount == 1)
                    {
                        weight = 50;
                    }else if (inMapCellCount == 2)
                    {
                        weight = 30;
                    }
                    else
                    {
                        weight = 20;
                    }
                    weightList.Add(weight);
                }

                var index = RandomHelper.RandomByWeight(weightList);
                var nextCell = nextCells[index];
                nextCell.IsInMap = true;
                generateCellIds.Add(nextCell.Id);
            }


            foreach (var generateCellId in generateCellIds)
            {
                var genCellData = allGenCellDataDict.GetValueOrDefault(generateCellId);
                genCells.Add(genCellData.CellData);
            }
            
            plantData.AllCells.Clear();
        }


        public static void GenerateCellReSize(PlantGenContext plantGenContext)
        {
            var plantData = plantGenContext.PlantData;
            
            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxX = 0;
            int maxY = 0;
            
            foreach (var realCell in plantData.RealCells)
            {
                foreach (var border in realCell.Borders)
                {
                    var x1 = border.x;
                    var y1 = border.y;
                    var x2 = border.z;
                    var y2 = border.w;

                    if (x1 < minX)
                    {
                        minX = (int)x1;
                    }
                    
                    if (x2 < minX)
                    {
                        minX = (int)x2;
                    }

                    if (y1 < minY)
                    {
                        minY = (int)y1;
                    }
                    
                    if (y2 < minY)
                    {
                        minY = (int)y2;
                    }
                    
                    if (x1 > maxX)
                    {
                        maxX = (int)x1;
                    }
                 
                    if (x2 > maxX)
                    {
                        maxX = (int)x2;
                    }
                    
                    if (y1 > maxY)
                    {
                        maxY = (int)y1;
                    }
                 
                    if (y2 > maxY)
                    {
                        maxY = (int)y2;
                    }
                }
            }
            
            
            // 进行Cell偏移
            foreach (var realCell in plantData.RealCells)
            {
                HashSet<float4> newBorders = new ();
                foreach (var border in realCell.Borders)
                {
                    newBorders.Add(new float4(border.x -minX , border.y - minY, border.z - minX, border.w - minY));
                }
                realCell.Center = new float2(realCell.Center.x - minX, realCell.Center.y - minY);
                realCell.Borders = newBorders;
            }
            
            plantData.MaxX = maxX - minX + 1;
            plantData.MaxY = maxY - minY + 1;

            plantData.MaxXTileCount = plantData.MaxX / BattleGlobalConfigCategory.Instance.Config.TileMapUnitSize + 1;
            plantData.MaxYTileCount = plantData.MaxY / BattleGlobalConfigCategory.Instance.Config.TileMapUnitSize + 1;
        }
        
        
        private static (List<FortuneSite>, LinkedList<VEdge>) GenerateFortuneSites(int areaWidth, Random random, int pointCount,
        int pointMinDistance = 5)
        {
            var fortuneSites = new List<FortuneSite>();
            var points = GenerateRandomPoint(areaWidth, random, pointCount, pointMinDistance);
            foreach (var point in points)
            {
                fortuneSites.Add(new FortuneSite(point.x, point.y));
            }

            var edges = FortunesAlgorithm.Run(fortuneSites, 0, 0, areaWidth, areaWidth);
            return (fortuneSites, edges);
        }

        /// <summary>
        /// 在一个 Area 中平均生成 一定数量的点
        /// </summary>
        /// <param name="areaWidth">整个地图的宽度</param>
        /// <param name="random">随机数生成器</param>
        /// <param name="pointCount">点的数量</param>
        /// <param name="pointMinDistance">点之间的最小距离</param>
        /// <returns></returns>
        public static List<float2> GenerateRandomPoint(int areaWidth, Random random, int pointCount, int pointMinDistance = 5)
        {
            List<float2> points = new List<float2>();
            List<float2> unitMeshes = new List<float2>();
            var pointSqrt = Math.Sqrt(pointCount);
            int divideCount = (int)pointSqrt;
            if (pointSqrt > (int)pointSqrt)
            {
                divideCount += 1;
            }

            var totalMeshCount = divideCount * divideCount;
            double area = areaWidth * areaWidth * 1.0f / totalMeshCount;
            float unitMeshWidth = (float)Math.Sqrt(area);

            for (float x = 0; x <= areaWidth - unitMeshWidth; x += unitMeshWidth)
            {
                for (float y = 0; y <= areaWidth - unitMeshWidth; y += unitMeshWidth)
                {
                    unitMeshes.Add(new float2(x, y));
                }
            }

            // 最多尝试 10000 次 防止死循环
            for (int i = 0; i < 10000; i++)
            {
                if (unitMeshes.Count <= 0)
                {
                    break;
                }

                var randomIndex = random.Next(unitMeshes.Count);
                var unitMesh = unitMeshes[randomIndex];
                unitMeshes.RemoveAt(randomIndex);

                float newX = (float)random.NextDouble() * unitMeshWidth + unitMesh.x;
                float newY = (float)random.NextDouble() * unitMeshWidth + unitMesh.y;

                bool tooClose = false;
                foreach (var point in points)
                {
                    var distance = Math.Sqrt(
                        Math.Abs(newX - point.x) * Math.Abs(newX - point.x) + Math.Abs(newY - point.y) * Math.Abs(newY - point.y));
                    if (distance < pointMinDistance)
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (!tooClose)
                {
                    points.Add(new float2(newX, newY));
                }

                if (points.Count >= pointCount)
                {
                    // 生成完成
                    break;
                }
            }

            return points;
        }
    }
}