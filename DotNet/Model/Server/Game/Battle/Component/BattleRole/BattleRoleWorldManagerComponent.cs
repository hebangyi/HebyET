namespace ET.Server;

[ComponentOf(typeof(BattleRole))]
public class BattleRoleWorldManagerComponent: Entity, IAwake, IDestroy
{
    public EntityRef<LogicWorld> World;
}