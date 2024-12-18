using System.Collections.Generic;
using MongoDB.Driver;

namespace ET.Server
{
    [ComponentConfigOf("MongoDBComponent")]
    public class MongoDBComponentConfig
    {
        public string DbConnection = "mongodb://localhost:27017/";
        public string DbName = "Unit";
    }


    public class MongoEntity : Entity
    {
    }

    [ComponentOf(typeof (Scene))]
    public class MongoDBComponent : Entity, IAwake
    {
        public MongoDBComponentConfig Config;
        public MongoClient MongoClient;
        public IMongoDatabase MongoDatabase;
        public HashSet<string> QueryIndexs = new();
    }
}