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

        public static async ETTask<int> SessionLogin(this ClientBattleSenderComponent self, string token)
        {
            self.fiberId = await FiberManager.Instance.Create(SchedulerType.ThreadPool, 0, SceneType.NetLobby, "");
            self.netClientActorId = new ActorId(self.Fiber().Process, self.fiberId);

            var request = Main2NetBattleLogin.Create();
            request.OwnerFiberId = self.Fiber().Id;
            request.Token = token;
            var response = await self.Root().GetComponent<ProcessInnerSender>().Call(self.netClientActorId, request) as NetBattle2MainLogin;
            return response.Error;
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
