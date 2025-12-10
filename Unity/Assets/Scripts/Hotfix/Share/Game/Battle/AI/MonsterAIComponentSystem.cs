namespace ET
{
    
    [FriendOf(typeof(MonsterAIComponent))]
    [EntitySystemOf(typeof(MonsterAIComponent))]
    public static partial class MonsterAIComponentSystem
    {
        [EntitySystem]
        private static void Awake(this MonsterAIComponent self)
        {
            MonsterAIAgentProxy monsterAIAgentProxy = new MonsterAIAgentProxy();
            var unitEntity = self.GetParent<UnitEntity>();
            monsterAIAgentProxy.UnitEntity = unitEntity;
            var logicWorld = unitEntity.LogicWorld();
            var aiComponent = logicWorld.GetComponent<AIComponent>();
            
            // 企业微信
            MonsterAIAgent monsterAIAgent = new MonsterAIAgent();
            monsterAIAgent.btsetcurrent("MonsterAITree");
            
            monsterAIAgent.AIAgentProxy = monsterAIAgentProxy;
            aiComponent.MonsterAIAgents[unitEntity.InsId] = monsterAIAgent;
        }

        [EntitySystem]
        private static void Destroy(this MonsterAIComponent self)
        {
        }
    }
}
