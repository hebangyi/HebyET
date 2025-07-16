using Unity.Mathematics;
using Random = System.Random;

namespace ET;

public class PointRandomTest
{
    public static void Test()
    {
        long unixTimeMilliseconds1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        var points = new PointGenerator().Generate(100);
        for (var i = 0; i < points.Count; i++)
        {
            Console.WriteLine($"点 {i + 1}: ({points[i].x}, {points[i].y})");
        }

        long unixTimeMilliseconds2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        Console.WriteLine($"花费时间 {unixTimeMilliseconds2 - unixTimeMilliseconds1}");
    }
}

public class PointGenerator
{
    // 生成图的宽度
    public int wight = 100;

    // 生成数量
    public int pointCount = 1000;
    public int minDistance = 1;

    public List<float2> Generate(int seed)
    {
        Random random = new Random(seed);
        List<float2> units = new List<float2>();
        List<float2> points = new List<float2>();

        var pointSqrt = Math.Sqrt(pointCount);
        int divideCount = (int)pointSqrt;
        if (pointSqrt > (int)pointSqrt)
        {
            divideCount += 1;
        }

        divideCount *= divideCount;

        double area = (this.wight * this.wight) * 1.0f / divideCount;
        float unitWidth = (float)Math.Sqrt(area);
        Console.WriteLine($"单位边长 : {unitWidth}");

        for (float x = 0; x <= wight - unitWidth; x += unitWidth)
        {
            for (float y = 0; y <= wight - unitWidth; y += unitWidth)
            {
                units.Add(new float2(x, y));
            }
        }

        // 最多尝试 10000 次 防止死循环
        for (int i = 0; i < 10000; i++)
        {
            if (units.Count <= 0)
            {
                break;
            }
            
            var randomIndex = random.Next(units.Count);
            var unitMesh = units[randomIndex];
            units.RemoveAt(randomIndex);

            float newX = (float)random.NextDouble() * unitWidth + unitMesh.x;
            float newY = (float)random.NextDouble() * unitWidth + unitMesh.y;

            bool tooClose = false;
            foreach (var point in points)
            {
                var distance = Math.Sqrt(Math.Abs(newX - point.x) * Math.Abs(newX - point.x) + Math.Abs(newY - point.y) * Math.Abs(newY - point.y));
                if (distance < this.minDistance)
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