using System.Collections.Generic;

namespace ET.Server;

[Event(SceneType.Lobby)]
public class RoleInfoComponent_LobbyRoleInit : AEvent<Scene, LobbyRoleDBInitEvent>
{
    protected override async ETTask Run(Scene scene, LobbyRoleDBInitEvent args)
    {
        var lobbyRole = args.LobbyRole;
        lobbyRole.InitData();
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
        
        role.RoleStatus = LobbyRoleStatus.Online;
        Log.Info($"玩家上线 Id : {role.RoleId}");
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
        role.RoleStatus = LobbyRoleStatus.OffOnline;
        role.LoginOutTime = roleInfoComponent.roleInfoData.LastLoginOutTime;
        Log.Info($"玩家下线 Id : {role.RoleId}");
        await ETTask.CompletedTask;
    }
}

public static class LobbyRoleHelper
{
    public static void InitData(this LobbyRole lobbyRole)
    {
        var roleInfoComponent = lobbyRole.GetComponent<RoleInfoComponent>();
        if (roleInfoComponent.roleInfoData == null)
        {
            roleInfoComponent.roleInfoData = new RoleInfoServerData();
        }
    }
    
    public static void SendToClient(this LobbyRole lobbyRole, IMessage message)
    {
        lobbyRole.GetComponent<EntityClientSessionComponent>()?.Session.Send(message);
    }

    public static void BindClientSession(this LobbyRole role, Session session)
    {
        // Session 与玩家相互绑定
        var entityClientSessionComponent = role.GetComponent<EntityClientSessionComponent>();
        var oldSession = entityClientSessionComponent.Session;

        if (oldSession != null)
        {
            // 已经用相同的Session 绑定过了
            if (oldSession.InstanceId == session.InstanceId)
            {
                return;
            }

            // 不同Session 则踢掉老链接
            KickOutOldPlayerSession(role, ErrorCode.OtherPersonLogin);
        }

        entityClientSessionComponent.Session = session;
        session.AddComponent<SessionLobbyPlayerComponent, long>(role.RoleId);
    }

    public static void KickOutOldPlayerSession(this LobbyRole role, int errorCode)
    {
        var entityClientSessionComponent = role.GetComponent<EntityClientSessionComponent>();
        var session = entityClientSessionComponent.Session;
        if (session == null)
        {
            return;
        }

        var message = G2C_SessionDisconnect.Create();
        message.Error = (int)errorCode;
        role.SendToClient(message);

        // 移除 该Session 绑定的 Player Component
        session.RemoveComponent<SessionLobbyPlayerComponent>();

        // 将Session 挂载在 自动销毁的定时器中
        session.AddComponent<SessionAcceptLoginCheckTimeoutComponent>();

    }
}
