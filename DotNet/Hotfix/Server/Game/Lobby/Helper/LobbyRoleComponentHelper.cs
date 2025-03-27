namespace ET.Server;


// 在线玩家秒级监听处理器
[Event(SceneType.Lobby)]
public class GlobalTimeOneSecond_CheckPlayerClockTime: AEvent<Scene, GlobalTimeOneSecond>
{
    protected override async ETTask Run(Scene scene, GlobalTimeOneSecond a)
    {
        var lobbyRoleComponent = scene.GetComponent<LobbyRoleComponent>();
        long now = TimeInfo.Instance.NowSec();
        foreach (var onlineRoleKv in lobbyRoleComponent.OnlineRoles)
        {
            LobbyRole lobbyRole = onlineRoleKv.Value;
            RoleClockComponentHelper.CheckPlayerClockTime(lobbyRole, now);
        }
        await ETTask.CompletedTask;
    }
}

public static class LobbyRoleComponentHelper
{
    public static void SendToClient(this LobbyRole lobbyRole, IMessage message)
    {
        lobbyRole.GetComponent<EntityClientSessionComponent>()?.Session.Send(message);
    }
    
    
    public static void BindClientSession(LobbyRole role, Session session)
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
            KickOutSessionPlayer(role, ErrorCode.OtherPersonLogin);
        }
        
        entityClientSessionComponent.Session = session;
        session.AddComponent<SessionLobbyPlayerComponent,long >(role.RoleId);
    }

    public static void KickOutSessionPlayer(LobbyRole role, ErrorCode errorCode)
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