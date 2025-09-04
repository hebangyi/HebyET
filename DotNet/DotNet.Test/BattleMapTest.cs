using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Numerics;
using ET;
using Unity.Mathematics;
using VoronoiLib;
using VoronoiLib.Structures;
using Random = System.Random;

namespace DotNet.Test;

public class BattleMapTest
{
    // 生成随机点集
    public static void Test()
    {
        int areaSize = 1000;
        int totalCellCount = 100;
        int remainCount = 15;
        var (pointSite, edges) = GenerateFortuneSites(areaSize, 0, totalCellCount, 5);
        
        Console.WriteLine(edges.Count);
        
        Dictionary<float2, Cell> pointCenter2Cells = new Dictionary<float2, Cell>();
        List<Cell> allCells = new List<Cell>();
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
                
            leftCell.CellEdges.Add( new float4((float)edge.Start.X, (float)edge.Start.Y, (float)edge.End.X, (float)edge.End.Y));
                
            var rightCell = pointCenter2Cells.GetValueOrDefault(rightPoint);
            if (rightCell == null)
            {
                rightCell = new Cell();
                rightCell.Center = rightPoint;
                pointCenter2Cells[rightPoint] = rightCell;
            }
                
            rightCell.CellEdges.Add( new float4((float)edge.Start.X, (float)edge.Start.Y, (float)edge.End.X, (float)edge.End.Y));
                
            leftCell.NearCells.Add(rightPoint);
            rightCell.NearCells.Add(leftPoint);
            allCells.Add(leftCell);
            allCells.Add(rightCell);
        }
        
        Console.WriteLine($"{pointSite.Count} {pointCenter2Cells.Count} ");
    }

    public static (List<FortuneSite>, LinkedList<VEdge>) GenerateFortuneSites(int areaWidth, int seed, int pointCount, int pointMinDistance = 5)
    {
        var fortuneSites = new List<FortuneSite>();
        var points = GenerateRandomPoint(areaWidth, seed, pointCount, pointMinDistance);
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
    /// <param name="seed">随机数的种子</param>
    /// <param name="pointCount">点的数量</param>
    /// <param name="pointMinDistance">点之间的最小距离</param>
    /// <returns></returns>
    public static List<float2> GenerateRandomPoint(int areaWidth, int seed, int pointCount, int pointMinDistance = 5)
    {
        var random = new Random(seed);
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