using System.Collections.Generic;

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

[Event(SceneType.Lobby)]
public class RoleInfoComponent_LobbyRoleOnlineEvent : AEvent<Scene, LobbyRoleOnlineEvent>
{
    protected override async ETTask Run(Scene scene, LobbyRoleOnlineEvent args)
    {
        var roleId = args.RoleId;
        LobbyRole role = scene.GetComponent<LobbyRoleComponent>().GetById(roleId);
        if (role == null)
        {
            return;
        }
        
        var roleInfoComponent = role.GetComponent<RoleInfoComponent>();
        roleInfoComponent.roleInfoData.LastLoginTime = TimeInfo.Instance.NowSec();
        await ETTask.CompletedTask;
    }
}

[Event(SceneType.Lobby)]
public class RoleInfoComponent_LobbyRoleOffOnlineEvent : AEvent<Scene, LobbyRoleOffOnlineEvent>
{
    protected override async ETTask Run(Scene scene, LobbyRoleOffOnlineEvent args)
    {
        var roleId = args.RoleId;
        LobbyRole role = scene.GetComponent<LobbyRoleComponent>().GetById(roleId);
        if (role == null)
        {
            return;
        }

        var roleInfoComponent = role.GetComponent<RoleInfoComponent>();
        roleInfoComponent.roleInfoData.LastLoginOutTime = TimeInfo.Instance.NowSec();
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
            roleInfoComponent.roleInfoData = new RoleInfoServerData();
        }
    }
    
}