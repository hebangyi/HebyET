namespace ET.Server;

// 用户定时器
[AutoAddComponent([typeof(LobbyRole)])]
[ComponentOf(typeof(LobbyRole))]
public class RoleClockComponent: Entity
{
    [MongoField("RoleClockData")]
    public RoleClockServerData RoleClockData;
}