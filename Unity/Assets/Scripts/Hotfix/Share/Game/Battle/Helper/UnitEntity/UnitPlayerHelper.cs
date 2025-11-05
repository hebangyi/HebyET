using System.Collections.Generic;
using Unity.Mathematics;

namespace ET
{
    public  static class UnitPlayerHelper
    {
        public static UnitEntity Create(LogicWorld logicWorld, long playerId)
        {
            var unitEntity = logicWorld.CreateEntity();
            var unitEntityCommonData = unitEntity.CreateUnitEntityElemData<UnitEntityCommonData>();
            unitEntityCommonData.UnitEntityType = UETypeEnum.Player;
            unitEntityCommonData.UEShowTypeEnum = UEShowTypeEnum.Player;
            
            UnitEntityInfo unitEntityInfo = unitEntity.CreateUnitEntityElemData<UnitEntityInfo>();
            unitEntityInfo.ConfigId = 0;
            unitEntityInfo.Speed = 3;
        
            var unitEntityPlayerInfo = unitEntity.CreateUnitEntityElemData<UnitEntityPlayerInfo>();
            unitEntityPlayerInfo.PlayerId = playerId;

            unitEntity.CreateUnitEntityElemData<UnitEntityPlayerData>();
            
            unitEntity.CreateUnitEntityElemData<UnitEntityTowardAngle>();
            
            var unitEntityPosition = unitEntity.CreateUnitEntityElemData<UnitEntityPosition>();
            unitEntityPosition.Position = new float2(0f, 0f);

            var unitEntityPlayerFrame = unitEntity.CreateUnitEntityElemData<UnitEntityPlayerFrame>();
            unitEntityPlayerFrame.Frame = logicWorld.Frame;

            unitEntity.CreateUnitEntityLogicElemData<UnitEntityPlayerOperation>();
            var unitEntityPlayerCellInfo = unitEntity.CreateUnitEntityLogicElemData<UnitEntityPlayerCellInfo>();
            
            logicWorld.CreateEntityFinish(unitEntity);
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

