namespace ET.Client
{
    
    [EntitySystemOf(typeof(ClientWorldManagerComponent))]
    [FriendOf(typeof(ClientWorldManagerComponent))]
    public static partial class ClientWorldManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ClientWorldManagerComponent self)
        {
            ClientWorldManagerComponent.Instance = self;
        }


        public static ClientWorld CreateWorld(this ClientWorldManagerComponent self)
        {
            ClientWorld world = self.AddChild<ClientWorld>();
            self.CurrentClientWorld = world;
            return world;
        }
    }
}

