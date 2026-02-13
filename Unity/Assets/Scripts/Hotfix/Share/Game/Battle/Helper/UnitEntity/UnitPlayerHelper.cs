using System.Collections.Generic;
using System.ComponentModel;
using Unity.Mathematics;

namespace ET
{
    public  static class UnitPlayerHelper
    {
        public static UnitEntity Create(LogicWorld logicWorld, long playerId)
        {
            return logicWorld.Create(UETypeEnum.Player, playerId);
        }
        
        public static UnitEntity GetPlayerUnitEntityByPlayerId(LogicWorld logicWorld, long playerId)
        {
            var unitEntity = logicWorld.PlayerId2Players.GetValueOrDefault(playerId);
            return unitEntity;
        }

        public static UnitEntity NearestEnemy(UnitEntity unitEntity)
        {
            var battleCellIds = BattleHelper.GetSearchBattleCellIds(unitEntity);
            var aoiManagerComponent = unitEntity.LogicWorld().GetComponent<AOIManagerComponent>();
            
            UnitEntity nearestPlayer = null;
            float nearestDistance = float.MaxValue;
            foreach (var battleCellId in battleCellIds)
            {
                var cellData = aoiManagerComponent.GetCellData(battleCellId);
                if (cellData == null)
                {
                    continue;
                }

                foreach (var allUnitEntity in cellData.AllUnitEntities.Values)
                {
                    var cellUnitEntity = allUnitEntity.GetParent<UnitEntity>();
                    if (unitEntity.InsId == cellUnitEntity.InsId)
                    {
                        continue;
                    }
                    var distance = BattleHelper.Distance(cellUnitEntity.GetUnitEntityElemData<UnitEntityPosition>().Position,
                        unitEntity.GetUnitEntityElemData<UnitEntityPosition>().Position);
                    if (nearestPlayer == null)
                    {
                        nearestPlayer = cellUnitEntity;
                        nearestDistance = distance;
                        continue;
                    }
                    
                    if (distance < nearestDistance)
                    {
                        nearestPlayer = cellUnitEntity;
                        nearestDistance = distance;
                    }
                }
            }

            return nearestPlayer;
        }
        
        
        
        public static void Online(LogicWorld logicWorld, long playerId)
        {
            var unitEntity = logicWorld.PlayerId2Players.GetValueOrDefault(playerId);
            var playerInfo = unitEntity?.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            if (playerInfo == null)
            {
                playerInfo.IsOnline = true;
                playerInfo.LastLoginTime = TimeInfo.Instance.NowSec();
            }
        }

        public static void Offline(LogicWorld logicWorld, long playerId)
        {
            var unitEntity = logicWorld.PlayerId2Players.GetValueOrDefault(playerId);
            var playerInfo = unitEntity?.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            if (playerInfo == null)
            {
                playerInfo.IsOnline = true;
                playerInfo.LastLoginTime = TimeInfo.Instance.NowSec();
            }
        }


    }
}

