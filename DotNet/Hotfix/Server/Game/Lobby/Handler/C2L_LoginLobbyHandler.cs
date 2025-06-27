using OfficeOpenXml.Export.ToDataTable;

namespace ET.Server
{
    [MessageSessionHandler(SceneType.Lobby)]
    [FriendOf(typeof(LobbyRole))]
    [FriendOf(typeof(SessionLobbyPlayerComponent))]
    [FriendOf(typeof(RoleInfoComponent))]
    public class C2L_LoginLobbyHandler : MessageSessionHandler<C2L_LoginLobby, L2C_LoginLobby>
    {
        protected override async ETTask Run(Session session, C2L_LoginLobby request, L2C_LoginLobby response)
        {
            var (ret, accountBean) = RSATokenManager.Instance.VerifyToken<AccountLoginRSA>(request.Token);
            if (!ret)
            {
                response.Error = (int)ErrorCode.LoginTokenErr;
                
                // TODO 断开链接 要注意消息要发送完
                return;
            }

            var roleId = accountBean.RoleId;
            Scene root = session.Root();
            session.RemoveComponent<SessionAcceptLoginCheckTimeoutComponent>();
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
                lobbyRole.AddComponent<EntityClientSessionComponent>();
                lobbyRole.AddComponent<LobbySyncUnitDataComponent>();
                // 设置网络邮箱与消息处理方式
                lobbyRole.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.UnOrderedMessage);
                lobbyRole.BindClientSession(session);
            }
            
            // 登录事件
            await EventSystem.Instance.PublishAsync(root, new LobbyRoleLogin1Event { LobbyRole = lobbyRole });
            await EventSystem.Instance.PublishAsync(root, new LobbyRoleLogin2Event { LobbyRole = lobbyRole });
            await EventSystem.Instance.PublishAsync(root, new LobbyRoleLogin3Event { LobbyRole = lobbyRole });
            // 登录完成事件
            await EventSystem.Instance.PublishAsync(root, new LobbyRoleLoginFinishedEvent { LobbyRole = lobbyRole });

            // 绑定Session可以发送消息
            lobbyRole.BindClientSession(session);
            
            response.PlayerId = lobbyRole.RoleId;
            await ETTask.CompletedTask;
            
        }
    }
}