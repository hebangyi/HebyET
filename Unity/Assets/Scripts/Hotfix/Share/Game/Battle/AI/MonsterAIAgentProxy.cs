using behaviac;

namespace ET
{
    [AIAgentProxy]
    public class MonsterAIAgentProxy : IMonsterAIAgentInterface
    {
        public UnitEntity UnitEntity { get; set; }

        public bool IsPatrolAction()
        {
            return true;
        }

        public bool IsFightAction()
        {
            return false;
        }

        // 追击
        public bool IsChaseAction()
        {
            return false;
        }

        public void DoChaseAction()
        {
            this.UnitEntity.GetComponent<MonsterStateMachineComponent>().ChangeState(MachineStateEnum.Chase);
        }

        public void DoFightAction()
        {
            this.UnitEntity.GetComponent<MonsterStateMachineComponent>().ChangeState(MachineStateEnum.Fight);
        }

        public void DoPatrolAction()
        {
            // Log.Info("做巡逻...");
            this.UnitEntity.GetComponent<MonsterStateMachineComponent>().ChangeState(MachineStateEnum.Patrol);
        }
    }
}