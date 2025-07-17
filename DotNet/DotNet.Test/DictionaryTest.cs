using Unity.Mathematics;
using Random = System.Random;

namespace DotNet.Test;

public class DictionaryTest
{
    public static void Test()
    {
        Random r = new Random();
        var A = r.NextDouble();
        var B = r.NextDouble();
        var C = r.NextDouble();
        var D = r.NextDouble();

        var A1 = new float2((float)A, (float)B);
        var A2 = new float2((float)A, (float)B);

        Dictionary<float2, int> dictionary = new Dictionary<float2, int>();
        dictionary.Add(A1, 1);
        dictionary.Add(A2, 1);
        
        Console.WriteLine((A1 == A2)[0]);

    }
}