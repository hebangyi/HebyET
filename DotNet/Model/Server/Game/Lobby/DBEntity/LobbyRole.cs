using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET.Server;

[ChildOf(typeof(LobbyRoleComponent))]
public class LobbyRole : Entity
{
    public long RoleId; // 玩家id
    
    public LobbyRole(Scene scene, long roleId)
    {
        this.Id = roleId;
        this.RoleId = roleId;
        this.InstanceId = IdGenerater.Instance.GenerateInstanceId();
        this.IsCreated = true;
        this.IsNew = true;
        this.IScene = scene;
        this.IsRegister = true;
    }
}

public class LobbyRoleEntity : MongoEntity
{

}