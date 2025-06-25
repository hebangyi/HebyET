namespace ET
{
    [EntitySystemOf(typeof(World))]
    [FriendOf(typeof(World))]
    public static partial class WorldSystem
    {
        [EntitySystem]
        private static void Awake(this World self)
        {
        }

        public static UnitEntity CreateEntity(this World self)
        {
            var unitEntity = self.AddChild<UnitEntity>();
            unitEntity.InsId = unitEntity.InstanceId;
            self.AllEntity[unitEntity.InsId] = unitEntity;
            return unitEntity;
        }

        public static void DestroyEntity(this World self, UnitEntity unitEntity)
        {
            self.AllEntity.Remove(unitEntity.InsId);
        }

        public static void Tick(this World self)
        {
            var comId2Logics = BattleUnitEntityDataLogicManagerComponent.Instance.CompId2TickLogics;
            foreach (var comId2LogicsKv in comId2Logics)
            {
                var logicHandlers = comId2LogicsKv.Value;
                foreach (var logicHandler in logicHandlers)
                {
                    logicHandler.OnTick();
                }
            }
        }
    }
}