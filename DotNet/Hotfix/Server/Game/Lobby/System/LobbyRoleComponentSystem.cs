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
}