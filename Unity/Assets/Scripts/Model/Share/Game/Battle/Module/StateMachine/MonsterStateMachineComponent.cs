namespace ET;

[ComponentOf(typeof(UnitEntity))]
public class MonsterStateMachineComponent: Entity, IAwake
{
    public StateMachineContext StateMachineContext;
}

public class StateMachineContext
{
    public MachineStateEnum CurrentState { get; set; }
}