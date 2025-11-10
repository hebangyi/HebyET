namespace ET.Client
{
    
    [EntitySystemOf(typeof(UnitySceneClientWorldManagerComponent))]
    [FriendOf(typeof(UnitySceneClientWorldManagerComponent))]
    public static partial class ClientWorldManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UnitySceneClientWorldManagerComponent self)
        {
            UnitySceneClientWorldManagerComponent.Instance = self;
        }

        
        [EntitySystem]
        private static void Destroy(this UnitySceneClientWorldManagerComponent self)
        {
            UnitySceneClientWorldManagerComponent.Instance = null;
        }

        

        public static ClientWorld CreateWorld(this UnitySceneClientWorldManagerComponent self)
        {
            ClientWorld world = self.AddChild<ClientWorld>();
            self.CurrentClientWorld = world;
            return world;
        }
    }
}

