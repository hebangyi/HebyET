namespace ET.Client
{
    [EntitySystemOf(typeof(ClientBattleSenderComponent))]
    [FriendOf(typeof(ClientBattleSenderComponent))]
    public static partial class ClientBattleSenderComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.ClientBattleSenderComponent self)
        {
            ClientBattleSenderComponent.Instance = self;
        }
        
        [EntitySystem]
        private static void Destroy(this ET.Client.ClientBattleSenderComponent self)
        {
            ClientBattleSenderComponent.Instance = null;
            self.RemoveFiberAsync().Coroutine();
        }
        
        
        private static async ETTask RemoveFiberAsync(this ClientBattleSenderComponent self)
        {
            if (self.fiberId == 0)
            {
                return;
            }

            int fiberId = self.fiberId;
            self.fiberId = 0;
            await FiberManager.Instance.Remove(fiberId);
        }
    }
}
