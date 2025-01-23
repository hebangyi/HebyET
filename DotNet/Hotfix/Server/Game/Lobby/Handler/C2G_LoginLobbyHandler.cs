using System;


namespace ET.Server
{
    [MessageSessionHandler(SceneType.Lobby)]
    [FriendOf(typeof(LobbyRole))]
    [FriendOf(typeof(SessionPlayerComponent))]
    public class C2G_LoginLobbyHandler : MessageSessionHandler<C2G_LoginLobby, G2C_LoginLobby>
    {
        protected override async ETTask Run(Session session, C2G_LoginLobby request, G2C_LoginLobby response)
        {
            Scene root = session.Root();
            session.RemoveComponent<SessionAcceptTimeoutComponent>();
            LobbyRoleComponent lobbyRoleComponent = root.GetComponent<LobbyRoleComponent>();

            var lobbyRole = lobbyRoleComponent.GetById(request.PlayerId);
            if (lobbyRole == null)
            {
                lobbyRole = new LobbyRole(session.Root(), request.PlayerId);
                PlayerSessionComponent playerSessionComponent = lobbyRole.AddComponent<PlayerSessionComponent>();
                
                // ?
                playerSessionComponent.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.GateSession);
                playerSessionComponent.Session = session;
                session.TryAddComponent<SessionPlayerComponent>().RoleId = request.PlayerId;
                lobbyRoleComponent.Add(lobbyRole);
            }
            else
            {
                KickOutOldPlayer(lobbyRole);
                PlayerSessionComponent playerSessionComponent = lobbyRole.GetComponent<PlayerSessionComponent>();
                playerSessionComponent.Session = session;
                session.TryAddComponent<SessionPlayerComponent>().RoleId = request.PlayerId;
            }
            
            // 
            
            response.PlayerId = lobbyRole.RoleId;
            await ETTask.CompletedTask;
        }

        private static void KickOutOldPlayer(LobbyRole lobbyRole)
        {
            var playerSessionComponent = lobbyRole.GetComponent<PlayerSessionComponent>();
            // TODO 退出 不是重连
            G2C_Reconnect g2CReconnect = G2C_Reconnect.Create();
            playerSessionComponent.Session.Send(g2CReconnect);
        }
    }
}