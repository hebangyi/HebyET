namespace ET.Server;

[ChildOf(typeof(BattleRoleComponent))]
public class BattleRole : Entity, IAwake
{
    public long RoleId; // 玩家id
    public BattleRoleStatus RoleStatus = BattleRoleStatus.Init; // 玩家状态
    public long LoginTime;
}


public enum BattleRoleStatus
{
    Init = 0,
    Online = 1, // 在线
    Offline = 2 // 离线
}
