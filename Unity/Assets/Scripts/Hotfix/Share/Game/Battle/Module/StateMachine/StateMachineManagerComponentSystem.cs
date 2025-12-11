using System;
using System.Reflection;
using ET.Client;

namespace ET
{
    
    [FriendOf(typeof(StateMachineManagerComponent))]
    [EntitySystemOf(typeof(StateMachineManagerComponent))]
    public static partial class StateMachineManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this StateMachineManagerComponent self)
        {
            StateMachineManagerComponent.Instance = self;
            var types = CodeTypes.Instance.GetAttributeTypes(typeof(MachineState));
            foreach (Type type in types)
            {
                IMachineState machineState = Activator.CreateInstance(type) as IMachineState;
                if (machineState == null)
                {
                    throw new Exception($"type not is BattleEvent: {type.Name}");
                }
                
                var machineStateAttribute = type.GetCustomAttribute(typeof(MachineState)) as MachineState;
                self.States[machineStateAttribute.MachineStateEnum] = machineState;
            }
        }
    }
}
