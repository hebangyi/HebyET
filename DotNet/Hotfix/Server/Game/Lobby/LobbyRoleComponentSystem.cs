using System.Collections.Generic;

namespace ET.Server;

[EntitySystemOf(typeof(LobbyRoleComponent))]
[FriendOf(typeof(LobbyRoleComponent))]
[FriendOf(typeof(LobbyRole))]
public static partial class LobbyRoleComponentSystem
{
    [EntitySystem]
    public static void Awake(this LobbyRoleComponent self)
    {
    }

    [EntitySystem]
    private static void Destroy(this ET.Server.LobbyRoleComponent self)
    {
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
}