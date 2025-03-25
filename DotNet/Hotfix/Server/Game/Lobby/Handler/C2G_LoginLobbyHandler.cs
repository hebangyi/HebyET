namespace ET.Server
{
    [MessageSessionHandler(SceneType.Lobby)]
    [FriendOf(typeof(LobbyRole))]
    [FriendOf(typeof(SessionPlayerComponent))]
    [FriendOf(typeof(RoleInfoComponent))]
    public class C2L_LoginLobbyHandler : MessageSessionHandler<C2L_LoginLobby, L2C_LoginLobby>
    {
        protected override async ETTask Run(Session session, C2L_LoginLobby request, L2C_LoginLobby response)
        {
            var (ret, accountBean) = RSATokenManager.Instance.VerifyToken<AccountLoginRSA>(request.Token);
            if (!ret)
            {
                response.Error = (int)ErrorCode.LoginTokenErr;
                return;
            }

            var roleId = accountBean.RoleId;
            Scene root = session.Root();
            session.RemoveComponent<SessionAcceptTimeoutComponent>();
            LobbyRoleComponent lobbyRoleComponent = root.GetComponent<LobbyRoleComponent>();

            var lobbyRole = lobbyRoleComponent.GetById(roleId);
            if (lobbyRole == null)
            {
                var mongoDbComponent = session.Fiber().Root.GetComponent<MongoDBComponent>();
                var lobbyRoleEntity = await mongoDbComponent.QueryOne<LobbyRoleEntity>(x => x.Id == roleId);
                bool isNewPlayer = false;
                if (lobbyRoleEntity == null)
                {
                    lobbyRoleEntity = new LobbyRoleEntity();
                    isNewPlayer = true;
                }
                
                lobbyRole = lobbyRoleComponent.Add(roleId);
                MongoEntityHelper.AttachData(lobbyRole, lobbyRoleEntity);

                // 抛出数据初始化事件
                await EventSystem.Instance.PublishAsync(root, new LobbyRoleDBInitEvent { LobbyRole = lobbyRole });
                
                if (isNewPlayer)
                {
                    lobbyRole.GetComponent<RoleInfoComponent>().roleInfoData.NickName = roleId.ToString();
                    await EventSystem.Instance.PublishAsync(root, new LobbyRoleNewPlayerEvent { LobbyRole = lobbyRole });
                }

                // 登录事件
                await EventSystem.Instance.PublishAsync(root, new LobbyRoleLogin1Event { LobbyRole = lobbyRole });
                await EventSystem.Instance.PublishAsync(root, new LobbyRoleLogin2Event { LobbyRole = lobbyRole });
                await EventSystem.Instance.PublishAsync(root, new LobbyRoleLogin3Event { LobbyRole = lobbyRole });
                // 登录完成事件
                await EventSystem.Instance.PublishAsync(root, new LobbyRoleLoginFinishedEvent { LobbyRole = lobbyRole });
                
                EntityClientSessionComponent entityClientSessionComponent = lobbyRole.AddComponent<EntityClientSessionComponent>();
                
                
                // 数据自动同步组件
                lobbyRole.TryAddComponent<LobbySyncUnitDataComponent>();
                
                // 绑定网络
                lobbyRole.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.OrderedMessage);
                entityClientSessionComponent.Session = session;
                var sessionPlayerComponent = session.TryAddComponent<SessionPlayerComponent>();
                sessionPlayerComponent.RoleId = roleId;
            }
            else
            {
                KickOutOldPlayer(lobbyRole);
                EntityClientSessionComponent entityClientSessionComponent = lobbyRole.GetComponent<EntityClientSessionComponent>();
                entityClientSessionComponent.Session = session;
                session.TryAddComponent<SessionPlayerComponent>().RoleId = roleId;
            }

            response.PlayerId = lobbyRole.RoleId;
            await ETTask.CompletedTask;
        }

        private static void KickOutOldPlayer(LobbyRole lobbyRole)
        {
            var sessionComponent = lobbyRole.GetComponent<EntityClientSessionComponent>();
            // TODO 退出 不是重连 session 被清掉
            G2C_Reconnect g2CReconnect = G2C_Reconnect.Create();
            sessionComponent.Session.Send(g2CReconnect);
        }
    }
}