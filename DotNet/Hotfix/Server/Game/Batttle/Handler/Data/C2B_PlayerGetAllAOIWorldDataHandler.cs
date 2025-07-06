namespace ET.Server.Data;

[MessageClientHandler(SceneType.Battle)]
public class C2B_PlayerGetAllAOIWorldDataHandler: MessageClientHandler<BattleRole, C2B_PlayerGetAllAOIWorldData, B2C_PlayerGetAllAOIWorldData>
{
    protected override void Run(BattleRole battleRole, C2B_PlayerGetAllAOIWorldData request, B2C_PlayerGetAllAOIWorldData response)
    {
        World world = battleRole.World();
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
        
        response.BattleWorld = world.ToBattleWorld();
        response.MyPlayerUnitEntity = unitPlayerEntity.ToBattleUnitEntity();
        
        
        // TODO AOI 机制 
        foreach (var unitEntityKv in world.AllEntity)
        {
            BattleUnitEntity battleUnitEntity = unitEntityKv.Value.ToBattleUnitEntity();
            response.BattleUnitEntity.Add(battleUnitEntity);
        }
        
        Log.Info($"C2B_PlayerGetAllAOIWorldData");
    }
}