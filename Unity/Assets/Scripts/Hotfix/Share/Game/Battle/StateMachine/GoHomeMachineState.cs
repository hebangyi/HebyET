using Unity.Mathematics;

namespace ET
{
    [MachineState(MachineStateEnum.GoHome)]
    public class GoHomeMachineState : IMachineState
    {
        public void Enter(MonsterStateMachineComponent component)
        {
            var unitEntity = component.GetParent<UnitEntity>();
            unitEntity.ChangeAnimateStatus(AnimateStateEnum.Walk);
        }

        public void Execute(MonsterStateMachineComponent component)
        {
            var unitEntity = component.GetParent<UnitEntity>();
            var logicWorld = unitEntity.LogicWorld();
            var monsterRuntimeData = unitEntity.GetUnitEntityLogicElemData<MonsterRuntimeData>();
            int speed = 5;
            unitEntity.GetUnitEntityElemData<UnitEntityPosition>().Position = UnitMonsterHelper.ToPosition(logicWorld,
                unitEntity.GetUnitEntityElemData<UnitEntityPosition>().Position, monsterRuntimeData.BornPosition, speed, out var _);
        }

        public void Exit(MonsterStateMachineComponent component)
        {
        }
    }
}