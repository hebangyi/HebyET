/*using ET;

namespace DotNet.Test.RandomTest;

using System;
using System.Collections.Generic;
using System.Drawing;

public class PoissonDiskSampler
{
    public static void Test()
    {
        var width = 800;
        var height = 800;
        var minDistance = 1; // 实际应用中可能需要调整此值

        var sampler = new PoissonDiskSampler(width, height, minDistance, 200);
        var points = sampler.Sample();

        Console.WriteLine($"生成了 {points.Count} 个点，最小距离: {minDistance}");
            
        // 示例：输出前10个点的坐标
        for (var i = 0; i < points.Count; i++)
        {
            Console.WriteLine($"点 {i + 1}: ({points[i].X}, {points[i].Y})");
        }
    }
    
    
    
    private readonly int width;
    private readonly int height;
    private readonly double minDistance;
    private readonly int maxAttempts;
    private readonly Random random;

    public PoissonDiskSampler(int width, int height, double minDistance, int maxAttempts = 30, int? seed = null)
    {
        this.width = width;
        this.height = height;
        this.minDistance = minDistance;
        this.maxAttempts = maxAttempts;
        this.random = seed.HasValue ? new Random(seed.Value) : new Random();
    }

    public List<PointF> Sample()
    {
        var cellSize = minDistance / Math.Sqrt(2);
        var gridWidth = (int)Math.Ceiling(width / cellSize);
        var gridHeight = (int)Math.Ceiling(height / cellSize);
        var grid = new PointF?[gridWidth, gridHeight];

        var points = new List<PointF>();
        var activePoints = new List<PointF>();

        // 添加初始点
        var firstPoint = new PointF((float)(random.NextDouble() * width), (float)(random.NextDouble() * height));
        points.Add(firstPoint);
        activePoints.Add(firstPoint);

        // 计算网格索引
        var gridX = (int)(firstPoint.X / cellSize);
        var gridY = (int)(firstPoint.Y / cellSize);
        grid[gridX, gridY] = firstPoint;

        // 生成剩余的点
        while (activePoints.Count > 0 && points.Count < 100)
        {
            // 随机选择一个活跃点
            var randomIndex = random.Next(activePoints.Count);
            var point = activePoints[randomIndex];

            // 尝试在周围生成新点
            var found = false;
            for (var i = 0; i < maxAttempts; i++)
            {
                var angle = random.NextDouble() * Math.PI * 2;
                var distance = random.NextDouble() * minDistance + minDistance;
                var newX = point.X + (float)(Math.Cos(angle) * distance);
                var newY = point.Y + (float)(Math.Sin(angle) * distance);

                // 检查是否在边界内
                if (newX >= 0 && newX < width && newY >= 0 && newY < height)
                {
                    // 检查与周围点的距离
                    var gridXNew = (int)(newX / cellSize);
                    var gridYNew = (int)(newY / cellSize);
                    var minX = Math.Max(0, gridXNew - 2);
                    var maxX = Math.Min(gridWidth - 1, gridXNew + 2);
                    var minY = Math.Max(0, gridYNew - 2);
                    var maxY = Math.Min(gridHeight - 1, gridYNew + 2);

                    var isValid = true;
                    for (var y = minY; y <= maxY; y++)
                    {
                        for (var x = minX; x <= maxX; x++)
                        {
                            if (grid[x, y].HasValue)
                            {
                                var neighbor = grid[x, y].Value;
                                var dx = newX - neighbor.X;
                                var dy = newY - neighbor.Y;
                                var dist = Math.Sqrt(dx * dx + dy * dy);
                                if (dist < minDistance)
                                {
                                    isValid = false;
                                    break;
                                }
                            }
                        }

                        if (!isValid) break;
                    }

                    if (isValid)
                    {
                        var newPoint = new PointF(newX, newY);
                        points.Add(newPoint);
                        activePoints.Add(newPoint);
                        grid[gridXNew, gridYNew] = newPoint;
                        found = true;
                        break;
                    }
                }
            }

            // 如果没有找到有效的新点，从活跃点列表中移除
            if (!found)
            {
                activePoints.RemoveAt(randomIndex);
            }
        }

        return points;
    }
}*/