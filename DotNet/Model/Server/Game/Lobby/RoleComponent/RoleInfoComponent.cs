namespace ET.Server;

[AutoAddComponent([typeof(LobbyRole)])]
[ComponentOf(typeof(LobbyRole))]
public class RoleInfoComponent : Entity
{
    [MongoField("RoleInfoData")]
    public RoleInfoData roleInfoData;
}

// 存储数据
public class RoleInfoData
{
    public string NickName;
}