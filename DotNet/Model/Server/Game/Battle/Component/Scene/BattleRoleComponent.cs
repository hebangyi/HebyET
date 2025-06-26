using System.Collections.Generic;

namespace ET.Server;
[ComponentOf(typeof(Scene))]
public class BattleRoleComponent:Entity, IAwake, IDestroy
{
    public Dictionary<long, EntityRef<BattleRole>> BattleRoles = new();
}