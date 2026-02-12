using ET;
using MongoDB.Bson;

namespace DotNet
{
    public static class JsonTest
    {
        public static void Test()
        {
            var ret = new BsonArray();
            ret.Add(1);
            ret.Add(2);
            ret.Add(3);

            Console.WriteLine(ret.ToString());
            Console.WriteLine(Enum.Parse(typeof(EnumA), "TestA"));
        }

        public static void Test1()
        {
            PA p = new PA();
            p.Name = "name";
            p.Age = 18;
            p.EnumA = new EnumA[] { EnumA.TestA, EnumA.TestB };

            PB pb = new PB();
            pb.A = "heby";
            p.PB = new PB[] { pb };

            Console.WriteLine(JsonHelper.ToJson(p));

            string ss = "{\"Name\" : \"name\", \"Age\" : 18, \"EnumA\" : [\"TestA\", \"TestB\"], \"PB\" : [{ \"A\" : \"heby\", \"B\" : 0 }] }";
            var p1 = JsonHelper.FromJson<PA>(ss);
            Console.WriteLine(p1.EnumA);
        }
    }

    public class PA
    {
        public string Name;
        public int Age;
        public EnumA[] EnumA;
        public PB[] PB;
    }

    public class PB
    {
        public string A;
        public int B;
    }

    public enum EnumA
    {
        TestA,
        TestB,
    }
}