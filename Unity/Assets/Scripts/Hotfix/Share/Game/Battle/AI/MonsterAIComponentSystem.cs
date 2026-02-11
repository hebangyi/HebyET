namespace ET
{
    
    [FriendOf(typeof(MonsterAIComponent))]
    [EntitySystemOf(typeof(MonsterAIComponent))]
    public static partial class MonsterAIComponentSystem
    {
        [EntitySystem]
        private static void Awake(this MonsterAIComponent self)
        {
            MonsterAIAgentProxy monsterAIAgentProxy = new();
            var unitEntity = self.GetParent<UnitEntity>();
            monsterAIAgentProxy.UnitEntity = unitEntity;
            // 行为树逻辑
            self.Proxy = monsterAIAgentProxy;
            // BT行为树
            MonsterAIAgent monsterAIAgent = new (monsterAIAgentProxy);
            monsterAIAgent.btsetcurrent("MonsterAITree");
            self.MonsterAIAgent = monsterAIAgent;
        }

        [EntitySystem]
        private static void Destroy(this MonsterAIComponent self)
        {
            // TODO AITree 能否复用 效率怎么样 
        }
    }
}
