using System;
using MongoDB.Driver.Core.Operations;
using Unity.Mathematics;

namespace ET
{
    public static class UnitMonsterHelper
    {
        public static UnitEntity Create(LogicWorld logicWorld, float2 position)
        {
            return logicWorld.Create(UETypeEnum.Monster, position);
        }

        /// <summary>
        /// 获得目标最近的玩家
        /// </summary>
        /// <param name="unitEntity"></param>
        /// <returns></returns>
        public static UnitEntity NearestPlayer(UnitEntity unitEntity)
        {
            var logicWorld = unitEntity.LogicWorld();

            UnitEntity nearestPlayer = null;
            float nearestDistance = float.MaxValue;
            foreach (var playerUnitEntity in logicWorld.PlayerId2Players.Values)
            {
                var distance = BattleHelper.Distance(playerUnitEntity.GetUnitEntityElemData<UnitEntityPosition>().Position,
                    unitEntity.GetUnitEntityElemData<UnitEntityPosition>().Position);
                if (nearestPlayer == null)
                {
                    nearestPlayer = playerUnitEntity;
                    nearestDistance = distance;
                    continue;
                }

                if (distance < nearestDistance)
                {
                    nearestPlayer = playerUnitEntity;
                    nearestDistance = distance;
                }
            }

            return nearestPlayer;
        }

        public static float BornDistance(UnitEntity unitEntity)
        {
            var monsterRuntimeData = unitEntity.GetUnitEntityLogicElemData<MonsterRuntimeData>();
            var currentPosition = unitEntity.GetUnitEntityElemData<UnitEntityPosition>().Position;
            var bornPosition = monsterRuntimeData.BornPosition;
            var distance = BattleHelper.Distance(currentPosition, bornPosition);
            return distance;
        }

        public static bool IsFightAction(UnitEntity unitEntity)
        {
            /*
            var unitEntityPosition = unitEntity.GetUnitEntityElemData<UnitEntityPosition>();
            var monsterRuntimeData = unitEntity.GetUnitEntityLogicElemData<MonsterRuntimeData>();
            if (BattleHelper.Distance(unitEntityPosition.Position, monsterRuntimeData.BornPosition) > 50)
            {
                return false;
            }*/

            var unitEntityPosition = unitEntity.GetUnitEntityElemData<UnitEntityPosition>();
            var logicWorld = unitEntity.LogicWorld();
            foreach (var playerUnitEntity in logicWorld.PlayerId2Players.Values)
            {
                var playerUnitEntityPosition = playerUnitEntity.GetUnitEntityElemData<UnitEntityPosition>();
                var distance = BattleHelper.Distance(unitEntityPosition.Position, playerUnitEntityPosition.Position);

                if (distance <= 1)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsChaseAction(UnitEntity unitEntity)
        {
            var unitEntityPosition = unitEntity.GetUnitEntityElemData<UnitEntityPosition>();
            var logicWorld = unitEntity.LogicWorld();
            foreach (var playerUnitEntity in logicWorld.PlayerId2Players.Values)
            {
                var playerUnitEntityPosition = playerUnitEntity.GetUnitEntityElemData<UnitEntityPosition>();
                var distance = BattleHelper.Distance(unitEntityPosition.Position, playerUnitEntityPosition.Position);
                if (distance <= 500)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsGoHomeAction(UnitEntity unitEntity)
        {
            // TODO 是否有仇恨者
            var bornDistance = BornDistance(unitEntity);
            if (bornDistance >= 1500)
            {
                return true;
            }

            return false;
        }

        public static float2 ToPosition(LogicWorld logicWorld, float2 fromPosition, float2 finalPosition, int speed)
        {
            var moveMax = speed * logicWorld.IntervalMillis;
            var distance = BattleHelper.Distance(fromPosition, finalPosition);
            var targetFraction = moveMax >= distance ? finalPosition
                    : math.normalize(finalPosition - fromPosition) * speed * logicWorld.IntervalMillis + fromPosition;

            if (MonsterCanMove(logicWorld, targetFraction))
            {
                return targetFraction;
            }

            var subX = (targetFraction - fromPosition).x;
            var subY = (targetFraction - fromPosition).y;

            if (Math.Abs(subX) > Math.Abs(subY))
            {
                if (MonsterCanMove(logicWorld, fromPosition + new float2(subX, 0)))
                {
                    return fromPosition + new float2(subX, 0);
                }

                if (MonsterCanMove(logicWorld, fromPosition + new float2(0, subY)))
                {
                    return fromPosition + new float2(0, subY);
                }
            }
            else
            {
                if (MonsterCanMove(logicWorld, fromPosition + new float2(0, subY)))
                {
                    return fromPosition + new float2(0, subY);
                }

                if (MonsterCanMove(logicWorld, fromPosition + new float2(subX, 0)))
                {
                    return fromPosition + new float2(subX, 0);
                }
            }

            // 不能移动
            return fromPosition;
        }

        public static bool MonsterCanMove(LogicWorld logicWorld, float2 targetFraction)
        {
            if (targetFraction.x < 0 || targetFraction.y < 0)
            {
                return false;
            }

            int xTileCount = (int)targetFraction.x / BattleGlobalConfigCategory.Instance.Config.TileMapUnitSize;
            int yTileCount = (int)targetFraction.y / BattleGlobalConfigCategory.Instance.Config.TileMapUnitSize;

            var plantRuntimeData = logicWorld.PlantMessageUnitEntity.GetUnitEntityLogicElemData<PlantRuntimeData>();
            if (xTileCount >= plantRuntimeData.MaxXTileCount || yTileCount >= plantRuntimeData.MaxYTileCount)
            {
                return false;
            }

            if (!plantRuntimeData.TitleMaps[xTileCount, yTileCount])
            {
                return false;
            }

            // TODO 其他的物理阻挡
            return true;
        }
    }
}