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
    public class MongoDBComponent : Entity, IAwake, IDestroy
    {
        public MongoDBComponentConfig Config;
        public MongoClient MongoClient;
        public IMongoDatabase MongoDatabase;
    }
}