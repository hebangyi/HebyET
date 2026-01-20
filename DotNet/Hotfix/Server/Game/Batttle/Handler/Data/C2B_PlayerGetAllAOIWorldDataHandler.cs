using System.Collections.Generic;

namespace ET.Server;

[MessageClientHandler(SceneType.Battle)]
public class C2B_PlayerGetAllAOIWorldDataHandler : MessageClientHandler<BattleRole, C2B_PlayerGetAllAOIWorldData, B2C_PlayerGetAllAOIWorldData>
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

        var playerAOISeeUnitEntity = unitPlayerEntity.GetComponent<PlayerAOISeeUnitEntity>();
        foreach (var manageEntityId in playerAOISeeUnitEntity.ManageEntityIds)
        {
            var unitEntity = logicWorld.GetUnitEntityByInsId(manageEntityId);
            if (unitEntity == null)
            {
                continue;
            }
            
            BattleUnitEntity battleUnitEntity = unitEntity.ToBattleUnitEntity();
            response.AOIBattleUnitEntity.Add(battleUnitEntity);
        }

        battleRole.LastSyncWorldFrame = logicWorld.Frame;
        
        // 加上我自己的Entity数据
        BattleUnitEntity myPlayerUnitEntity = unitPlayerEntity.ToBattleUnitEntity();
        response.AOIBattleUnitEntity.Add(myPlayerUnitEntity);
        
        // 常规 AOI
        response.BattleFieldUnitEntity.Add(logicWorld.PlantMessageUnitEntity.ToBattleUnitEntity());
        response.BattleFieldUnitEntity.Add(logicWorld.GizmosDebugUnitEntity.ToBattleUnitEntity());
        response.BattleWorld.Frame = battleRole.LastSyncWorldFrame;
        response.LogicInterval = GameConstant.LogicInterval;
    }
}