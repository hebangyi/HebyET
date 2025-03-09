using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET.Server;

[ChildOf(typeof(LobbyRoleComponent))]
public class LobbyRole : Entity,IAwake
{
    public long RoleId; // 玩家id
    
}

public class LobbyRoleEntity : MongoEntity
{

}