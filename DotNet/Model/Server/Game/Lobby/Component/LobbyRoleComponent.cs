using System.Collections.Generic;
using System.ComponentModel;

namespace ET.Server;


[ComponentOf(typeof(Scene))]
public class LobbyRoleComponent :Entity, IAwake, IDestroy
{
    // 当前在线的玩家
    public Dictionary<long, EntityRef<LobbyRole>> Online2Roles = new();
}