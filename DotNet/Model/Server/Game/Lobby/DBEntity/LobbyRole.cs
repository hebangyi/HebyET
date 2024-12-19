using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET.Server;

public class LobbyRole : Entity
{
    public long RoleId; // 玩家id
    
    public LobbyRole(Fiber fiber, long roleId)
    {
        this.Id = roleId;
        this.RoleId = roleId;
        this.InstanceId = IdGenerater.Instance.GenerateInstanceId();
        this.IsCreated = true;
        this.IsNew = true;
        this.IScene = fiber.Root;
        this.IsRegister = true;
    }
    
}

public class LobbyRoleInfo : MongoEntity
{
    public long RoleId;
    public string NickName;
    //通用数据结构
    [BsonDictionaryOptions(DictionaryRepresentation.Document)]
    public Dictionary<string, object> DataCollections = new ();
}