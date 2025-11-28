namespace ET.Server;

[MessageClientHandler(SceneType.Battle)]
public class C2B_PlayerGetAllAOIWorldDataHandler: MessageClientHandler<BattleRole, C2B_PlayerGetAllAOIWorldData, B2C_PlayerGetAllAOIWorldData>
{
    protected override void Run(BattleRole battleRole, C2B_PlayerGetAllAOIWorldData request, B2C_PlayerGetAllAOIWorldData response)
    {
        LogicWorld logicWorld = battleRole.World();
        if (logicWorld == null)
        {
            response.Error = ErrorCode.NotFoundBattleWorld;
            return;
        }

        var unitPlayerEntity = UnitPlayerHelper.GetPlayerUnitEntityByPlayerId(logicWorld, battleRole.RoleId);
        if (unitPlayerEntity == null)
        {
            response.Error = ErrorCode.NotFoundWorldPlayer;
            return;
        }
        
        response.BattleWorld = logicWorld.ToBattleWorld();
        response.MyPlayerUnitEntity = unitPlayerEntity.ToBattleUnitEntity();
        var myAOIUnitEntity = unitPlayerEntity.GetComponent<AOIUnitEntity>();
        
        var aoiManagerComponent = logicWorld.GetComponent<AOIManagerComponent>();
        // 视野范围内的AOI
        var watchAOIUnitEntities = aoiManagerComponent.GetAllWatchUnitEntities(myAOIUnitEntity);
        foreach (var aoiUnitEntity in watchAOIUnitEntities)
        {
            var unitEntity = aoiUnitEntity.GetParent<UnitEntity>();
            BattleUnitEntity battleUnitEntity = unitEntity.ToBattleUnitEntity();
            response.AOIBattleUnitEntity.Add(battleUnitEntity);
        }

        // 常规 AOI
        response.BattleFieldUnitEntity.Add(logicWorld.PlantMessageUnitEntity.ToBattleUnitEntity());
        response.BattleFieldUnitEntity.Add(logicWorld.GizmosDebugUnitEntity.ToBattleUnitEntity());
        Log.Info($"C2B_PlayerGetAllAOIWorldData");
    }
}