namespace ET
{
    [MachineState(MachineStateEnum.Fight)]
    public class FightMachineState : IMachineState
    {
        public void Enter(MonsterStateMachineComponent component)
        {
            var unitEntity = component.GetParent<UnitEntity>();
            unitEntity.ChangeAnimateStatus(AnimateStateEnum.Attack);
        }

        public void Execute(MonsterStateMachineComponent component)
        {
        }

        public void Exit(MonsterStateMachineComponent component)
        {
        }
    }
}