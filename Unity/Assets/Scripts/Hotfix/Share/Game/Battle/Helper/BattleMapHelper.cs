using System;
using System.Collections.Generic;
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

            if (!IsSimpleFilter(targetPoint, polygon))
                return false;
            
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
        
        
        
        
        
        public static (List<FortuneSite>, LinkedList<VEdge>) GenerateFortuneSites(int areaWidth, Random random, int pointCount, int pointMinDistance = 5)
        {
            var fortuneSites = new List<FortuneSite>();
            var points = GenerateRandomPoint(areaWidth, random, pointCount, pointMinDistance);
            foreach (var point in points)
            {
                fortuneSites.Add(new FortuneSite(point.x, point.y));
            }
            var edges = FortunesAlgorithm.Run(fortuneSites, 0, 0, 800, 800);
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
                    var distance = Math.Sqrt(Math.Abs(newX - point.x) * Math.Abs(newX - point.x) + Math.Abs(newY - point.y) * Math.Abs(newY - point.y));
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