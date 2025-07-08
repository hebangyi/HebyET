namespace ET.Server;

[MessageClientHandler(SceneType.Battle)]
public class C2B_PlayerMoveOperationMessageHandler: MessageClientHandler<BattleRole, C2B_PlayerMoveOperationMessage>
{
    protected override void Run(BattleRole battleRole, C2B_PlayerMoveOperationMessage message)
    {
        var world = battleRole.World();

        if (world == null)
        {
            return;
        }
        
        var unitPlayerEntity = UnitPlayerHelper.GetPlayerUnitEntityByPlayerId(world, battleRole.RoleId);
        if (unitPlayerEntity == null)
        {
            return;
        }

        var unitEntityPlayerOperation = unitPlayerEntity.GetUnitEntityLogicElemData<UnitEntityPlayerOperation>();
        if (unitEntityPlayerOperation == null)
        {
            return;
        }

        unitEntityPlayerOperation.MoveAngel = message.MoveAngle;
    }
}