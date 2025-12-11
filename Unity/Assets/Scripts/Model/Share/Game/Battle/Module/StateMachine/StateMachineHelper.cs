using System.Collections.Generic;

namespace ET
{
    public static class StateMachineHelper
    {
        public static void ChangeState(this MonsterStateMachineComponent monsterStateMachineComponent,MachineStateEnum newState)
        {
            var machineContext = monsterStateMachineComponent.StateMachineContext;
            var currentState = machineContext.CurrentState;
            if (currentState == newState)
            {
                return;
            }

            var currentStateContext = StateMachineManagerComponent.Instance.States.GetValueOrDefault(currentState);
            if (currentStateContext != null)
            {
                currentStateContext.Exit(machineContext);
            }

            machineContext.CurrentState = newState;
            
            var newStateContext = StateMachineManagerComponent.Instance.States.GetValueOrDefault(newState);
            if (newStateContext != null)
            {
                newStateContext.Enter(machineContext);
            }
        }
    }
}

