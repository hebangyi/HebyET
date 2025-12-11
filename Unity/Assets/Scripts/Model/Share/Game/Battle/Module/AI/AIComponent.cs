using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(LogicWorld))]
    public class AIComponent: Entity, IAwake, IDestroy
    {
        public LogicWorld LogicWorld { get; set; }
        
        // 管理的所有 AIAgent
        public Dictionary<long, MonsterAIAgent> MonsterAIAgents = new Dictionary<long, MonsterAIAgent>();
    }

    [ComponentOf(typeof(UnitEntity))]
    public class MonsterAIComponent : Entity, IAwake, IDestroy
    {
        public IMonsterAIAgentInterface Proxy;
    }
    

}