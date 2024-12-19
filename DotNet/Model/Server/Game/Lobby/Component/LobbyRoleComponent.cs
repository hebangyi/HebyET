using System.Collections.Generic;

namespace ET.Server;

public class LobbyRoleComponent
{
    // 当前在线的玩家
    public Dictionary<long, LobbyRole> Online2Roles = new();
}