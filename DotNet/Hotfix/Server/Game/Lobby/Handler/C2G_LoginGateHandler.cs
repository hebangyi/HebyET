using System;


namespace ET.Server
{
    [MessageSessionHandler(SceneType.Lobby)]
    [FriendOfAttribute(typeof(ET.Server.LobbyRole))]
    [FriendOfAttribute(typeof(ET.Server.SessionPlayerComponent))]
    public class C2G_LoginGateHandler : MessageSessionHandler<C2G_LoginLobby, G2C_LoginLobby>
    {
        protected override async ETTask Run(Session session, C2G_LoginLobby request, G2C_LoginLobby response)
        {
            Scene root = session.Root();
            session.RemoveComponent<SessionAcceptTimeoutComponent>();
            LobbyRoleComponent lobbyRoleComponent = root.GetComponent<LobbyRoleComponent>();

            var lobbyRole = lobbyRoleComponent.GetById(request.PlayerId);
            if (lobbyRole == null)
            {
                lobbyRole = new LobbyRole();

                // 
                player = playerComponent.AddChild<Player, string>(account);
                playerComponent.Add(player);
                PlayerSessionComponent playerSessionComponent = player.AddComponent<PlayerSessionComponent>();
                playerSessionComponent.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.GateSession);
                await playerSessionComponent.AddLocation(LocationType.GateSession);

                player.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.UnOrderedMessage);
                await player.AddLocation(LocationType.Player);

                
                playerSessionComponent.Session = session;
                session.TryAddComponent<SessionPlayerComponent>().RoleId = request.PlayerId;
            }
            else
            {
                // 判断是否在战斗
                KickOutOldPlayer(lobbyRole);
                PlayerSessionComponent playerSessionComponent = lobbyRole.GetComponent<PlayerSessionComponent>();
                playerSessionComponent.Session = session;
                session.TryAddComponent<SessionPlayerComponent>().RoleId = request.PlayerId;
            }

            response.PlayerId = lobbyRole.RoleId;
            await ETTask.CompletedTask;
        }

        private static void KickOutOldPlayer(LobbyRole lobbyRole)
        {
            var playerSessionComponent = lobbyRole.GetComponent<PlayerSessionComponent>();

            // TODO 去除
            G2C_Reconnect g2CReconnect = G2C_Reconnect.Create();
            playerSessionComponent.Session.Send(g2CReconnect);
        }
    }
}