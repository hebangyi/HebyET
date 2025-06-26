using System;
using System.Collections.Generic;
using System.Linq;

namespace ET.Server;

// 在线玩家秒级监听处理器
[Event(SceneType.Lobby)]
public class GlobalTimeOneSecond_CheckPlayerClockTime : AEvent<Scene, GlobalTimeOneSecond>
{
    protected override async ETTask Run(Scene scene, GlobalTimeOneSecond a)
    {
        var lobbyRoleComponent = scene.GetComponent<LobbyRoleComponent>();
        long now = TimeInfo.Instance.NowSec();
        foreach (var onlineRoleKv in lobbyRoleComponent.OnlineRoles)
        {
            LobbyRole lobbyRole = onlineRoleKv.Value;
            if (lobbyRole == null)
            {
                continue;
            }

            RoleClockComponentHelper.CheckPlayerClockTime(lobbyRole, now);
        }

        await ETTask.CompletedTask;
    }
}

[Event(SceneType.Lobby)]
public class GlobalTimeTenSecond_CheckOnlineUnloadDB : AEvent<Scene, GlobalTimeTenSecond>
{
    protected override async ETTask Run(Scene scene, GlobalTimeTenSecond args)
    {
        var lobbyRoleComponent = scene.GetComponent<LobbyRoleComponent>();
        lobbyRoleComponent.CheckOffOnlineUnloadDB();
        lobbyRoleComponent.CheckUnloadedDB();
        await ETTask.CompletedTask;
    }
}


[Event(SceneType.Lobby)]
public class MongoAutoSaveEvent_MongoEntitySaveFinish : AEvent<Scene, MongoAutoSaveEvent>
{
    protected override async ETTask Run(Scene scene, MongoAutoSaveEvent args)
    {
        if (args.EntityType != typeof(LobbyRoleEntity))
        {
            return;
        }
        
        var lobbyRoleComponent = scene.GetComponent<LobbyRoleComponent>();

        foreach (var id in args.Ids)
        {
            var lobbyRole = lobbyRoleComponent.GetById(id);
            // 状态已经被其他的改变 则不处理
            if (lobbyRole.RoleStatus != LobbyRoleStatus.UnloadingDB)
            {
                continue;
            }
            lobbyRole.RoleStatus= LobbyRoleStatus.UnloadedDB;
        }
        await ETTask.CompletedTask;
    }
}


public static class LobbyRoleComponentHelper
{
    public static void CheckOffOnlineUnloadDB(this LobbyRoleComponent self)
    {
        try
        {
            foreach (var roleKv in self.OnlineRoles)
            {
                LobbyRole role = roleKv.Value;
                if (role.RoleStatus == LobbyRoleStatus.OffOnline)
                {
                    var lobbyRoleEntity = MongoEntityHelper.UnAttachData<LobbyRoleEntity>(role);
                    self.Root().GetComponent<MongoAutoSaveComponent>().AddSaveEntity(lobbyRoleEntity);
                    // 正在卸载
                    role.RoleStatus = LobbyRoleStatus.UnloadingDB;
                }
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
        }
    }


    public static void CheckUnloadedDB(this LobbyRoleComponent self)
    {
        try
        {
            List<long> unloadedIds = new List<long>();
            foreach (var roleKv in self.OnlineRoles)
            {
                LobbyRole role = roleKv.Value;
                if (role == null)
                {
                    unloadedIds.Add(role.RoleId);
                } else if (role.RoleStatus == LobbyRoleStatus.UnloadedDB)
                {
                    unloadedIds.Add(role.RoleId);
                }
            }
        
            foreach (var unloadId in unloadedIds)
            {
                if (self.OnlineRoles.Remove(unloadId, out var role))
                {
                    LobbyRole r = role;
                    r?.Dispose();
                }

                Log.Info($"玩家已经卸载 : {unloadId}");
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
        }
    }
    
    public static LobbyRole Add(this LobbyRoleComponent self, long roleId)
    {
        if (self.GetById(roleId) != null)
        {
            return self.GetById(roleId);
        }

        var lobbyRole = self.AddChildWithId<LobbyRole>(roleId);
        lobbyRole.RoleId = roleId;
        self.OnlineRoles[lobbyRole.RoleId] = lobbyRole;
        return lobbyRole;
    }

    public static LobbyRole GetById(this LobbyRoleComponent self, long roleId)
    {
        return self.OnlineRoles.GetValueOrDefault(roleId);
    }

    public static void Remove(this LobbyRoleComponent self, long roleId)
    {
        var lobbyRole = self.GetById(roleId);
        if (lobbyRole != null)
        {
            self.OnlineRoles.Remove(roleId);
            lobbyRole.Dispose();
        }
    }

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
            KickOutOldPlayerSession(role, ErrorCode.OtherPersonLogin);
        }

        entityClientSessionComponent.Session = session;
        session.AddComponent<SessionLobbyPlayerComponent, long>(role.RoleId);
    }

    public static void KickOutOldPlayerSession(LobbyRole role, int errorCode)
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