using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Driver;

namespace ET.Server
{
    [ComponentConfigOf("MongoDBComponent")]
    public class MongoDBComponentConfig
    {
        public string DbConnection = "mongodb://localhost:27017/";
        public string DbName = "Unit";
    }


    public interface IMongoEntityInterface
    {
        // byte[] Serialize();
        // void Deserialize(byte[] bytes);
    }

    public abstract class MongoEntity :IMongoEntityInterface
    {
        public long Id;
        //通用数据结构
        [BsonDictionaryOptions(DictionaryRepresentation.Document)]
        public Dictionary<string, object> DataCollections = new ();
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