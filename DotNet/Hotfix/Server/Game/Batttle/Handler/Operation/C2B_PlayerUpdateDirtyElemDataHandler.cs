using System.Collections.Generic;

namespace ET.Server;

[MessageClientHandler(SceneType.Battle)]
public class C2B_PlayerUpdateDirtyElemDataHandler: MessageClientHandler<BattleRole, C2B_PlayerUpdateDirtyElemData, B2C_PlayerUpdateDirtyElemData>
{
    protected override void Run(BattleRole battleRole, C2B_PlayerUpdateDirtyElemData request, B2C_PlayerUpdateDirtyElemData response)
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

        if (request.BattleUnitEntity == null)
        {
            return;
        }
        
        if (unitPlayerEntity.InsId != request.BattleUnitEntity.InsId)
        {
            return;
        }
        
        Log.Info("ClientInput...");
        
        LogicWorldHelper.ClientInput(unitPlayerEntity, request.BattleUnitEntity);
    }
}