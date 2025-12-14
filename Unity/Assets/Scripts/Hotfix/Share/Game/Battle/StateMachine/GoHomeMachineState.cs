using Unity.Mathematics;

namespace ET
{
    [MachineState(MachineStateEnum.GoHome)]
    public class GoHomeMachineState: IMachineState
    {
        public void Enter(MonsterStateMachineComponent component)
        {
        }

        public void Execute(MonsterStateMachineComponent component)
        {
            var unitEntity = component.GetParent<UnitEntity>();
            var logicWorld = unitEntity.LogicWorld();
            var monsterRuntimeData = unitEntity.GetUnitEntityLogicElemData<MonsterRuntimeData>();
            var unitEntityPosition = unitEntity.GetUnitEntityElemData<UnitEntityPosition>();
            var distance = BattleHelper.Distance(unitEntityPosition.Position, monsterRuntimeData.BornPosition);
            
            int speed = 5;
            var moveMax = speed * logicWorld.IntervalMillis;
            if (moveMax >= distance)
            {
                unitEntity.GetUnitEntityElemData<UnitEntityPosition>().Position = monsterRuntimeData.BornPosition;
            }
            else
            {
                float2 sub = monsterRuntimeData.BornPosition - unitEntityPosition.Position;
                var subDistance = math.normalize(sub) * speed * logicWorld.IntervalMillis;
                unitEntity.GetUnitEntityElemData<UnitEntityPosition>().Position += subDistance;
            }
            
        }

        public void Exit(MonsterStateMachineComponent component)
        {
        }
    }
}

