using behaviac;

namespace ET
{
    [AIAgentProxy]
    public class MonsterAIAgentProxy : IMonsterAIAgentInterface
    {
        public UnitEntity UnitEntity { get; set; }

        public EBTStatus IsEnemyInSight()
        {
            return UnitMonsterHelper.IsEnemyInSight(UnitEntity)? EBTStatus.BT_SUCCESS: EBTStatus.BT_FAILURE;
        }

        public EBTStatus IsInAttackRange()
        {
            return UnitMonsterHelper.IsInAttackRange(UnitEntity)? EBTStatus.BT_SUCCESS: EBTStatus.BT_FAILURE;
        }
        
        
        public void DoChaseAction()
        {
            Log.Info("做追击...");
        }

        public void DoFightAction()
        {
            Log.Info("做打击...");
        }

        public void DoPatrolAction()
        {
            Log.Info("做巡逻...");
        }
    }
}