using System.ComponentModel;

namespace ET.Server;

[AutoAddComponent([typeof(LobbyRole)])]
[ComponentOf(typeof(LobbyRole))]
public class RoleInfoComponent : Entity
{
    [MongoField("RoleInfoData")]
    public RoleInfoData roleInfoData;
}


public class RoleInfoData
{
    public string NickName;
}