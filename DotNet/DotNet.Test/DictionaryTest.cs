using ET;
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


    public static void Test2()
    {
        HashSet<int> hash = new HashSet<int>();
        hash.Add(1);
        hash.Add(2);
        hash.Add(3);
        hash.Add(4);
        hash.Add(5);
        hash.Add(6);
        hash.Add(7);
        hash.Add(8);
        hash.Add(9);
        hash.Add(10);
        hash.Add(11);
        hash.Add(12);
        hash.Add(13);


        foreach (var h in hash)
        {
            Console.WriteLine(h);
        }

        List<int> newList = new List<int>(hash);
        Console.WriteLine(JsonHelper.ToJson(newList));
    }
    
    public static void Test4()
    {
        HashSet<Person> persons = new HashSet<Person>();
        persons.Add(new Person(){Name = "1"});
        persons.Add(new Person(){Name = "2"});
        persons.Add(new Person(){Name = "3"});
        persons.Add(new Person(){Name = "4"});
        persons.Add(new Person(){Name = "5"});
        persons.Add(new Person(){Name = "6"});
        persons.Add(new Person(){Name = "7"});
        persons.Add(new Person(){Name = "8"});
        persons.Add(new Person(){Name = "9"});
        persons.Add(new Person(){Name = "10"});
        persons.Add(new Person(){Name = "11"});
        

        foreach (var h in persons)
        {
            Console.WriteLine(h.Name);
        }

        List<Person> newList = new List<Person>(persons);
        Console.WriteLine(JsonHelper.ToJson(newList));
    }

    public static void Test5()
    {
        bool[] array = new bool[10];
        foreach (var a in array)
        {
            Console.WriteLine(a);
        }
    }
    
    
    public class Person
    {
        public string Name;
    }
}