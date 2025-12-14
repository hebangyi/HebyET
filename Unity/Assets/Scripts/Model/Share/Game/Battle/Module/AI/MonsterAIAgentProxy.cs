namespace ET
{
    public interface IMonsterAIAgentInterface
    {
        bool IsPatrolAction();
        bool IsFightAction();
        bool IsChaseAction();

        bool IsGoHomeAction();
        
        
        void DoChaseAction();
        void DoFightAction();
        void DoPatrolAction();
        void DoGoHomeAction();
        

    }
    
    public class AIAgentProxy : BaseAttribute
    {
    }
}