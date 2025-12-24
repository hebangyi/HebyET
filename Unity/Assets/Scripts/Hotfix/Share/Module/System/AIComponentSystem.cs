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
            self.InitBehavior();
        }

        [EntitySystem]
        private static void Destroy(this ET.AIComponent self)
        {
        }


        public static void UpdateAITick(this ET.AIComponent self)
        {
            foreach (var aiAgent in self.MonsterAIAgents)
            {
                var status = aiAgent.Value.btexec();
                // Log.Info($"AI Status : {status}");
            }
        }

        public static void InitBehavior(this ET.AIComponent self)
        {
            behaviac.Workspace.Instance.FileFormat = behaviac.Workspace.EFileFormat.EFF_cs;
        }
    }
}