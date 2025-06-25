namespace ET.Server;

// 当前玩家战斗组件
[ComponentOf(typeof(LobbyRole))]
public class LobbyRoleBattleComponent: Entity,IAwake
{
    public long WorldId { get; set; }
}