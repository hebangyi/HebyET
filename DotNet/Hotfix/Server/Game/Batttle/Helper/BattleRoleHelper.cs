using System.Collections.Generic;

namespace ET.Server;

[Event(SceneType.Lobby)]
public class BattleRoleComponent_BattleRoleOnlineEvent : AEvent<Scene, BattleRoleOnlineEvent>
{
    protected override async ETTask Run(Scene scene, BattleRoleOnlineEvent args)
    {
        var roleId = args.RoleId;
        BattleRole role = scene.GetComponent<BattleRoleComponent>().GetById(roleId);
        if (role == null)
        {
            return;
        }

        role.LoginTime = TimeInfo.Instance.NowSec();
        role.RoleStatus = BattleRoleStatus.Online;
        Log.Info($"战斗服 玩家上线 Id : {role.RoleId}");
        await ETTask.CompletedTask;
    }
}

[Event(SceneType.Lobby)]
public class BattleRoleComponent_BattleRoleOffOnlineEvent : AEvent<Scene, BattleRoleOffOnlineEvent>
{
    protected override async ETTask Run(Scene scene, BattleRoleOffOnlineEvent args)
    {
        var roleId = args.RoleId;
        BattleRole role = scene.GetComponent<BattleRoleComponent>().GetById(roleId);
        if (role == null)
        {
            return;
        }


        role.RoleStatus = BattleRoleStatus.Offline;
        Log.Info($"战斗服 玩家下线 Id : {role.RoleId}");
        await ETTask.CompletedTask;
    }
}



public static class BattleRoleHelper
{
    public static void SendToClient(this BattleRole battleRole, IMessage message)
    {
        battleRole.GetComponent<EntityClientSessionComponent>()?.Session.Send(message);
    }

    public static BattleRole Add(this BattleRoleComponent self, long roleId)
    {
        var battleRole = self.GetById(roleId);
        if (battleRole != null)
        {
            return battleRole;
        }

        battleRole = self.AddChildWithId<BattleRole>(roleId);
        battleRole.RoleId = roleId;
        self.BattleRoles[battleRole.RoleId] = battleRole;
        return battleRole;
    }


    public static void BindClientSession(this BattleRole battleRole, Session session)
    {
        // Session 与玩家相互绑定
        var entityClientSessionComponent = battleRole.GetComponent<EntityClientSessionComponent>();
        var oldSession = entityClientSessionComponent.Session;
        if (oldSession != null)
        {
            // 已经用过相同的Session 进行绑定过了
            if (oldSession.InstanceId == session.InstanceId)
            {
                return;
            }
            
            // 不同Session 则踢掉老链接
            KickOutOldPlayerSession(battleRole, ErrorCode.OtherPersonLogin);
        }

        entityClientSessionComponent.Session = session;
        session.AddComponent<SessionBattlePlayerComponent, long>(battleRole.RoleId);
    }


    public static void KickOutOldPlayerSession(BattleRole role, int errorCode)
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
        session.RemoveComponent<SessionBattlePlayerComponent>();
        
        // 将Session 挂载在 自动销毁的定时器中
        session.AddComponent<SessionAcceptLoginCheckTimeoutComponent>();
    }


    public static BattleRole GetById(this BattleRoleComponent self, long roleId)
    {
        return self.BattleRoles.GetValueOrDefault(roleId);
    }
}