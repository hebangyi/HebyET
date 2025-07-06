using System.Collections.Generic;
using Unity.Mathematics;

namespace ET
{
    public  static class UnitPlayerHelper
    {
        public static UnitEntity Create(World world, long playerId)
        {
            BattleUnitEntity battleUnitEntity = BattleUnitEntity.Create();

            var unitEntity = world.AddChild<UnitEntity>();
            var unitEntityCommonData = unitEntity.GetOrCreateUnitEntityElemData<UnitEntityCommonData>();
            unitEntityCommonData.UnitEntityType = UnitEntityTypeEnum.Player;
            
            UnitEntityInfo unitEntityInfo = unitEntity.GetOrCreateUnitEntityElemData<UnitEntityInfo>();
            unitEntityInfo.UnitEntityTypeEnum = UnitEntityTypeEnum.Player;
            unitEntityInfo.ConfigId = 0;
        
            var unitEntityPlayerInfo = unitEntity.GetOrCreateUnitEntityElemData<UnitEntityPlayerInfo>();
            unitEntityPlayerInfo.PlayerId = playerId;
            
            var unitEntityPosition = unitEntity.GetOrCreateUnitEntityElemData<UnitEntityPosition>();
            unitEntityPosition.Forward = new int3(0, 0, 1);
            unitEntityPosition.Position = new int3(50, 0, 50);

            var unitEntityPlayerFrame = unitEntity.GetOrCreateUnitEntityElemData<UnitEntityPlayerFrame>();
            unitEntityPlayerFrame.Frame = world.Frame;
            
            UnitEntity entity = world.CreateEntity(unitEntity);
            return entity;
        }

        public static UnitEntity GetPlayerUnitEntityByPlayerId(World world, long playerId)
        {
            var unitEntity = world.AllPlayers.GetValueOrDefault(playerId);
            return unitEntity;
        }

        public static void Online(World world, long playerId)
        {
            var unitEntity = world.AllPlayers.GetValueOrDefault(playerId);
            var playerInfo = unitEntity?.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            if (playerInfo == null)
            {
                playerInfo.IsOnline = true;
                playerInfo.LastLoginTime = TimeInfo.Instance.NowSec();
            }
        }

        public static void Offline(World world, long playerId)
        {
            var unitEntity = world.AllPlayers.GetValueOrDefault(playerId);
            var playerInfo = unitEntity?.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            if (playerInfo == null)
            {
                playerInfo.IsOnline = true;
                playerInfo.LastLoginTime = TimeInfo.Instance.NowSec();
            }
        }
    }
}

