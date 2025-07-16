namespace DotNet.Test.RandomTest;

public class RandomTest
{
    public static void Test()
    {
        Random random = new Random(1000);
        for (int i = 0; i <= 100; i++)
        {
            Console.WriteLine(random.Next());
        }
    }
}