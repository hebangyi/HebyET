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
        self.AddChild(lobbyRole);
    }

    public static LobbyRole GetById(this LobbyRoleComponent self, long roleId)
    {
        return self.GetChild<LobbyRole>(roleId);
    }

    public static void Remove(this LobbyRoleComponent self, long roleId)
    {
        var lobbyRole = self.GetById(roleId);
        if (lobbyRole != null)
        {
            lobbyRole.Dispose();
        }
    }

}