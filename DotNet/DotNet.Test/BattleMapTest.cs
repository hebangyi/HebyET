using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Numerics;
using VoronoiLib;
using VoronoiLib.Structures;

namespace DotNet.Test;

public class BattleMapTest
{
    // 生成随机点集
    public static LinkedList<VEdge> GenerateRandomPoints()
    {
        var points = new List<FortuneSite>
        {
            new FortuneSite(100, 200),
            new FortuneSite(500, 200),
            new FortuneSite(300, 300),
            new FortuneSite(400, 400)
        };
        
        //FortunesAlgorithm.Run(points, min x, min y, max x, max y)
        var edges = FortunesAlgorithm.Run(points, 0, 0, 800, 800);
        return edges;
        //VEdge.Start is a VPoint with location VEdge.Start.X and VEdge.End.Y
        //VEdge.End is the ending point for the edge
        //FortuneSite.Neighbors contains the site's neighbors in the Delaunay Triangulation
    }

    /*
    // 计算Voronoi图
    static List<FortuneEdge> ComputeVoronoi(List<Vector2> points, double minX, double minY, double maxX, double maxY)
    {
        var sites = new List<Site>();
        foreach (var point in points)
        {
            sites.Add(new Site(point.X, point.Y));
        }

        // 扩展边界以确保生成完整的Voronoi图
        double margin = Math.Max(maxX - minX, maxY - minY) * 0.5;
        var bounds = new Rect(minX - margin, minY - margin, maxX + margin, maxY + margin);

        // 使用Fortune算法计算Voronoi图
        var voronoi = Fortune.ComputeVoronoiGraph(sites, bounds);
        return voronoi.Edges;
    }

    // 可视化并保存Voronoi图
    static void SaveVoronoiDiagram(List<FortuneEdge> edges, int width, int height, string filePath)
    {
        using (var bitmap = new Bitmap(width, height))
        using (var g = Graphics.FromImage(bitmap))
        {
            // 清空背景
            g.Clear(Color.White);

            // 绘制Voronoi边
            using (var pen = new Pen(Color.Blue, 1))
            {
                foreach (var edge in edges)
                {
                    if (edge.VVertexA != null && edge.VVertexB != null)
                    {
                        g.DrawLine(pen,
                            (float)edge.VVertexA.X, (float)edge.VVertexA.Y,
                            (float)edge.VVertexB.X, (float)edge.VVertexB.Y);
                    }
                }
            }

            // 保存图像
            bitmap.Save(filePath);
        }
    }*/
}