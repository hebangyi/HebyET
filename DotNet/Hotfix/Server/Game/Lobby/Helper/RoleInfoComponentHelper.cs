namespace ET.Server;

[Event(SceneType.Lobby)]
public class RoleInfoComponent_LobbyRoleInit : AEvent<Scene, LobbyRoleDBInitEvent>
{
    protected override async ETTask Run(Scene scene, LobbyRoleDBInitEvent args)
    {
        var lobbyRole = args.LobbyRole;
        RoleInfoComponentHelper.InitData(lobbyRole);
        await ETTask.CompletedTask;
    }
}

[FriendOf(typeof(RoleInfoComponent))]
public static class RoleInfoComponentHelper
{
    public static void InitData(LobbyRole lobbyRole)
    {
        var roleInfoComponent = lobbyRole.GetComponent<RoleInfoComponent>();
        if (roleInfoComponent.roleInfoData == null)
        {
            roleInfoComponent.roleInfoData = new RoleInfoData();
        }
    }
}