namespace ET.Server
{
    [MessageSessionHandler(SceneType.Battle)]
    public class C2B_LoginHandler : MessageSessionHandler<C2B_Login, B2C_Login>
    {
        protected override async ETTask Run(Session session, C2B_Login request, B2C_Login response)
        {
            var (ret, bean) = RSATokenManager.Instance.VerifyToken<BattleLoginRSA>(request.Token);
            if (!ret)
            {
                response.Error = (int)ErrorCode.LoginTokenErr;
                return;
            }
            long worldId = bean.WorldId;
            var world = BattleWorldManagerComponent.Instance.GetWorldById(worldId);
            if (world == null)
            {
                response.Error = (int)ErrorCode.NotFoundBattleNode;
                return;
            }


            // 登录成功
            session.RemoveComponent<SessionAcceptLoginCheckTimeoutComponent>();
            long playerId = bean.RoleId;

            Scene root = session.Root();
            BattleRoleComponent battleRoleComponent = root.GetComponent<BattleRoleComponent>();
            var battleRole = battleRoleComponent.GetByRoleId(playerId);
            if (battleRole == null)
            {
                battleRole = battleRoleComponent.Add(playerId);
                battleRole.AddComponent<EntityClientSessionComponent>();
                battleRole.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.UnOrderedMessage);
            }
            
            battleRole.BindClientSession(session);
            
            var battleRoleWorldManagerComponent = battleRole.TryAddComponent<BattleRoleWorldManagerComponent>();
            battleRoleWorldManagerComponent.World = world;
            await ETTask.CompletedTask;
        }
    }
}
