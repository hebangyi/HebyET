using System.Collections.Generic;
using Unity.Mathematics;

namespace ET
{
    public  static class UnitPlayerHelper
    {
        public static UnitEntity Create(World world, long playerId)
        {
            var unitEntity = world.CreateEntity();
            var unitEntityCommonData = unitEntity.CreateUnitEntityElemData<UnitEntityCommonData>();
            unitEntityCommonData.UnitEntityType = UnitEntityTypeEnum.Player;
            
            UnitEntityInfo unitEntityInfo = unitEntity.CreateUnitEntityElemData<UnitEntityInfo>();
            unitEntityInfo.UnitEntityTypeEnum = UnitEntityTypeEnum.Player;
            unitEntityInfo.ConfigId = 0;
            unitEntityInfo.Speed = 3;
        
            var unitEntityPlayerInfo = unitEntity.CreateUnitEntityElemData<UnitEntityPlayerInfo>();
            unitEntityPlayerInfo.PlayerId = playerId;
            
            var unitEntityPosition = unitEntity.CreateUnitEntityElemData<UnitEntityPosition>();
            unitEntityPosition.Forward = new int3(0, 0, 1);
            unitEntityPosition.Position = new int3(50, 0, 50);

            var unitEntityPlayerFrame = unitEntity.CreateUnitEntityElemData<UnitEntityPlayerFrame>();
            unitEntityPlayerFrame.Frame = world.Frame;
            
            
            var unitEntityPlayerOperationAction = unitEntity.CreateUnitEntityElemData<UnitEntityPlayerOperationAction>();
            unitEntityPlayerOperationAction.MoveAngle = -1;

            unitEntity.CreateUnitEntityLogicElemData<UnitEntityPlayerOperation>();
            
            world.CreateEntityFinish(unitEntity);
            return unitEntity;
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

