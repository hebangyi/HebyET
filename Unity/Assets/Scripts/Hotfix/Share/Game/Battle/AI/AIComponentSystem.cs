namespace ET
{
    [FriendOf(typeof(AIComponent))]
    [EntitySystemOf(typeof(AIComponent))]
    public static partial class AIComponentSystem
    {
        [EntitySystem]
        private static void Awake(this AIComponent self)
        {
            self.LogicWorld = self.GetParent<LogicWorld>();
        }

        [EntitySystem]
        private static void Destroy(this ET.AIComponent self)
        {
        }


        public static void UpdateAITick(this ET.AIComponent self)
        {
            foreach (var aiAgent in self.AIAgents)
            {
                aiAgent.Value.btexec();
            }
        }
    }
}