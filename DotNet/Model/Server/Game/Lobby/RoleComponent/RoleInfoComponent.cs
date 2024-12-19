using System.ComponentModel;

namespace ET.Server;

[AutoAddComponent([typeof(LobbyRole)])]
[ComponentOf(typeof(LobbyRole))]
public class RoleInfoComponent : Entity
{
    [RoleField("RoleInfoData")]
    public RoleInfoData roleInfoData;
}


public class RoleInfoData
{
    public string NickName;
}