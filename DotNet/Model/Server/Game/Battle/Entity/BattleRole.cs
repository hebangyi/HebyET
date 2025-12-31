namespace ET.Server;

[ChildOf(typeof(BattleRoleComponent))]
public class BattleRole : Entity, IAwake
{
    public long RoleId; // 玩家id
    public BattleRoleStatus RoleStatus = BattleRoleStatus.Init; // 玩家状态
    public long LoginTime;
    public uint LastSyncWorldFrame; // 上一次世界同步的世界帧
}


public enum BattleRoleStatus
{
    Init = 0,
    Online = 1, // 在线
    Offline = 2 // 离线
}
