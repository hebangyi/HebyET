namespace ET
{
    [ComponentOf(typeof(UnitEntity))]
    public class MonsterStateMachineComponent: Entity, IAwake
    {
        public MachineStateEnum CurrentState { get; set; }
    }
}
