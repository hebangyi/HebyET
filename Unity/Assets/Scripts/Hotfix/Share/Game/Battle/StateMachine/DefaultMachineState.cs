namespace ET
{
    [MachineState(MachineStateEnum.Default)]
    public class DefaultMachineState : IMachineState
    {
        public void Enter(StateMachineContext context)
        {
            Log.Info("Enter Default");
        }

        public void Execute(StateMachineContext context)
        {
            Log.Info("Execute Default");
        }

        public void Exit(StateMachineContext context)
        {
            Log.Info("Exit Default");
        }
    }
}

