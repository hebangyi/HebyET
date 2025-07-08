namespace ET.Server;

public class C2B_PlayerMoveOperationHandler: MessageClientHandler<BattleRole, C2B_PlayerMoveOperation, B2C_PlayerMoveOperation>
{
    protected override void Run(BattleRole battleRole, C2B_PlayerMoveOperation request, B2C_PlayerMoveOperation response)
    {
        var world = battleRole.World();

        if (world == null)
        {
            response.Error = ErrorCode.NotFoundBattleWorld;
            return;
        }
        
        var unitPlayerEntity = UnitPlayerHelper.GetPlayerUnitEntityByPlayerId(world, battleRole.RoleId);
        if (unitPlayerEntity == null)
        {
            response.Error = ErrorCode.NotFoundWorldPlayer;
            return;
        }
        
        
    }
}