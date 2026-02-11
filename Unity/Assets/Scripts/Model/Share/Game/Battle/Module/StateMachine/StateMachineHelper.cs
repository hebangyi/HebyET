using System.Collections.Generic;

namespace ET
{
    public static class StateMachineHelper
    {
        public static void ChangeState(this MonsterStateMachineComponent monsterStateMachineComponent, MachineStateEnum newState)
        {
            var currentState = monsterStateMachineComponent.CurrentState;
            if (currentState == newState)
            {
                return;
            }

            var currentStateContext = StateMachineManagerComponent.Instance.States.GetValueOrDefault(currentState);
            if (currentStateContext != null)
            {
                currentStateContext.Exit(monsterStateMachineComponent);
            }

            monsterStateMachineComponent.CurrentState = newState;
            
            var newStateContext = StateMachineManagerComponent.Instance.States.GetValueOrDefault(newState);
            if (newStateContext != null)
            {
                newStateContext.Enter(monsterStateMachineComponent);
            }
        }

        public static void Execute(this MonsterStateMachineComponent monsterStateMachineComponent)
        {
            var currentState = monsterStateMachineComponent.CurrentState;
            var currentStateContext = StateMachineManagerComponent.Instance.States.GetValueOrDefault(currentState);
            if (currentStateContext != null)
            {
                currentStateContext.Execute(monsterStateMachineComponent);
            }
        }
    }
}

