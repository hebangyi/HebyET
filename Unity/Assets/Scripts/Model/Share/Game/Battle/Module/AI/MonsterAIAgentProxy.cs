namespace ET
{
    public interface IMonsterAIAgentInterface
    {
        void DoChaseAction();
        void DoFightAction();
        void DoPatrolAction();
        
        behaviac.EBTStatus IsEnemyInSight();
        behaviac.EBTStatus IsInAttackRange();
    }
    
    public class AIAgentProxy : BaseAttribute
    {
    }
}