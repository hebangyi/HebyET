using Unity.Mathematics;

namespace ET
{
    public static class UnitMonsterHelper
    {
        public static UnitEntity Create(LogicWorld logicWorld, float2 position)
        {
            return logicWorld.Create(UETypeEnum.Monster, position);
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
                    Log.Info("IsInAttackRange...");
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
                if (distance <= 20)
                {
                    Log.Info("IsEnemyInSight...");
                    return true;
                }
            }
            return false;
        }
    }
}