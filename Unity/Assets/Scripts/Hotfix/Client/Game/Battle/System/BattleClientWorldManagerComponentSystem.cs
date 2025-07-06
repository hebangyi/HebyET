namespace ET.Client
{
    
    [EntitySystemOf(typeof(BattleClientWorldManagerComponent))]
    [FriendOf(typeof(BattleClientWorldManagerComponent))]
    public static partial class BattleClientWorldManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this BattleClientWorldManagerComponent self)
        {
            BattleClientWorldManagerComponent.Instance = self;
        }


        public static World CreateWorld(this BattleClientWorldManagerComponent self)
        {
            World world = self.AddChild<World, int>((int)WorldMode.View);
            self.CurrentWorld = world;
            return world;
        }
    }
}

