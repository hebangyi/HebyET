using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class StateMachineManagerComponent: Entity, IAwake
    {
        public static StateMachineManagerComponent Instance { get; set; }
        public Dictionary<MachineStateEnum, IMachineState> States = new Dictionary<MachineStateEnum, IMachineState>();
    }
    
    public enum MachineStateEnum
    {
        Default = 0,
        Idle = 1,               // Idle
        Rest = 2,               // 休息
        Patrol = 3,             // 巡逻
        Chase = 4,              // 追击
        Fight = 5,              // 攻击
    }
    
    public interface IMachineState
    {
        void Enter(MonsterStateMachineComponent component);
        
        void Execute(MonsterStateMachineComponent component);
        
        void Exit(MonsterStateMachineComponent component);
    }
    
    public class MachineState : BaseAttribute
    {
        public MachineStateEnum MachineStateEnum;
        
        public MachineState(MachineStateEnum machineStateEnum)
        {
            this.MachineStateEnum = machineStateEnum;
        }
    }
    

}