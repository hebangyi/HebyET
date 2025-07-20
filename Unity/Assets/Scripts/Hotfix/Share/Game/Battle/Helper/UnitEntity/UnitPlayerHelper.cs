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
            
            
            // 随机选择一个地块
            var unitEntityPlant = world.AllPlants[world.RandomGenerator.Next(world.AllPlants.Count)];
            var planeCellInfo = unitEntityPlant.GetUnitEntityElemData<PlaneCellInfo>();
            
            var unitEntityPosition = unitEntity.CreateUnitEntityElemData<UnitEntityPosition>();
            unitEntityPosition.Forward = new float3(0, 0, 1);
            unitEntityPosition.Position = new float3(planeCellInfo.Center.x, 0, planeCellInfo.Center.y);

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
            var unitEntity = world.PlayerId2Players.GetValueOrDefault(playerId);
            return unitEntity;
        }

        public static void Online(World world, long playerId)
        {
            var unitEntity = world.PlayerId2Players.GetValueOrDefault(playerId);
            var playerInfo = unitEntity?.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            if (playerInfo == null)
            {
                playerInfo.IsOnline = true;
                playerInfo.LastLoginTime = TimeInfo.Instance.NowSec();
            }
        }

        public static void Offline(World world, long playerId)
        {
            var unitEntity = world.PlayerId2Players.GetValueOrDefault(playerId);
            var playerInfo = unitEntity?.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            if (playerInfo == null)
            {
                playerInfo.IsOnline = true;
                playerInfo.LastLoginTime = TimeInfo.Instance.NowSec();
            }
        }
    }
}

