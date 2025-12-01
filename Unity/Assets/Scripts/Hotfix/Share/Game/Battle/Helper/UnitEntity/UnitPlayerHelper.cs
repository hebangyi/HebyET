using System.Collections.Generic;
using Unity.Mathematics;

namespace ET
{
    public  static class UnitPlayerHelper
    {
        public static UnitEntity Create(LogicWorld logicWorld, long playerId)
        {
            var unitEntity = logicWorld.CreateEntity();
            var playerInitInfo = unitEntity.AddComponent<PlayerInitContext>();
            playerInitInfo.PlayerId = playerId;
            logicWorld.CreateEntityFinish(unitEntity, UETypeEnum.Player);
            return unitEntity;
        }
        
        
        public static UnitEntity GetPlayerUnitEntityByPlayerId(LogicWorld logicWorld, long playerId)
        {
            var unitEntity = logicWorld.PlayerId2Players.GetValueOrDefault(playerId);
            return unitEntity;
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

