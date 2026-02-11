using System.Collections.Generic;

namespace ET
{

    [ComponentOf(typeof(UnitEntity))]
    public class MonsterAIComponent : Entity, IAwake, IDestroy
    {
        public IMonsterAIAgentInterface Proxy;

        public MonsterAIAgent MonsterAIAgent;
    }
    

}