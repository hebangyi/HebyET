namespace ET.Server;


[AutoAddComponent([typeof(LobbyRole)])]
[ComponentOf(typeof(LobbyRole))]
public class RoleInfoComponent : Entity
{
    
    
    
    [MongoField("RoleInfoData")]
    public RoleInfoServerData roleInfoData;
}