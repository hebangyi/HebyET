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
    
    public static void Add(this LobbyRoleComponent self, LobbyRole lobbyRole)
    {
        self.Online2Roles[lobbyRole.RoleId] = lobbyRole;
    }

    public static LobbyRole GetById(this LobbyRoleComponent self, long roleId)
    {
        return self.Online2Roles.GetValueOrDefault(roleId);
    }

    public static void Remove(this LobbyRoleComponent self, long roleId)
    {
        self.Online2Roles.Remove(roleId);
    }

}