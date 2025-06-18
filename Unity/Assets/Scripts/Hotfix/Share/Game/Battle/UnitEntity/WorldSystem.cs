namespace ET
{
    [EntitySystemOf(typeof(World))]
    [FriendOf(typeof(World))]
    public static partial class WorldSystem
    {
        [EntitySystem]
        private static void Awake(this ET.World self)
        {
        }

        public static UnitEntity CreateEntity(this ET.World self)
        {
            var unitEntity = self.AddChild<UnitEntity>();
            unitEntity.InsId = unitEntity.InstanceId;
            self.AllEntity[unitEntity.InsId] = unitEntity;
            return unitEntity;
        }

        public static void RemoveEntity(this World self, UnitEntity unitEntity)
        {
            self.AllEntity.Remove(unitEntity.InsId);
        }
    }
}