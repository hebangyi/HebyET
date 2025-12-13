namespace ET
{
    public interface IMonsterAIAgentInterface
    {
        bool IsPatrolAction();
        bool IsFightAction();
        bool IsChaseAction();
        
        
        void DoChaseAction();
        void DoFightAction();
        void DoPatrolAction();
        

    }
    
    public class AIAgentProxy : BaseAttribute
    {
    }
}