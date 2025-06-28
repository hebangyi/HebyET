namespace ET.Server;

// 当前玩家战斗组件
[ComponentOf(typeof(Session))]
public class SessionBattlePlayerComponent: Entity,IAwake<long>,IDestroy
{
    public long RoleId;
}