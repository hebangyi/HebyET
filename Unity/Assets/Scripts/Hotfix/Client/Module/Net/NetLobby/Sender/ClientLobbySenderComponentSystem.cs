using System.Threading.Tasks;
using CommandLine;

namespace ET.Client
{
    [EntitySystemOf(typeof(ClientLobbySenderComponent))]
    [FriendOf(typeof(ClientLobbySenderComponent))]
    public static partial class ClientLobbySenderComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ClientLobbySenderComponent self)
        {
            ClientLobbySenderComponent.Instance = self;
        }
        
        [EntitySystem]
        private static void Destroy(this ClientLobbySenderComponent self)
        {
            ClientLobbySenderComponent.Instance = null;
            self.RemoveFiberAsync().Coroutine();
        }

        private static async ETTask RemoveFiberAsync(this ClientLobbySenderComponent self)
        {
            if (self.fiberId == 0)
            {
                return;
            }

            int fiberId = self.fiberId;
            self.fiberId = 0;
            await FiberManager.Instance.Remove(fiberId);
        }

        public static async ETTask<(int,long)> LoginAsync(this ClientLobbySenderComponent self, string account, string password)
        {
            self.fiberId = await FiberManager.Instance.Create(SchedulerType.ThreadPool, 0, SceneType.NetLobby, "");
            self.netClientActorId = new ActorId(self.Fiber().Process, self.fiberId);

            Main2NetLobbyLogin main2NetClientLoginHandler = Main2NetLobbyLogin.Create();
            main2NetClientLoginHandler.OwnerFiberId = self.Fiber().Id;
            main2NetClientLoginHandler.Account = account;
            main2NetClientLoginHandler.Password = password;
            NetLobby2MainLogin response = await self.Root().GetComponent<ProcessInnerSender>().Call(self.netClientActorId, main2NetClientLoginHandler) as NetLobby2MainLogin;
            return (response.Error, response.PlayerId);
        }

        public static void Send(this ClientLobbySenderComponent self, IMessage message)
        {
            A2NetClient_Message a2NetClientMessage = A2NetClient_Message.Create();
            a2NetClientMessage.MessageObject = message;
            self.Root().GetComponent<ProcessInnerSender>().Send(self.netClientActorId, a2NetClientMessage);
        }

        public static async ETTask<IResponse> Call(this ClientLobbySenderComponent self, IRequest request, bool needException = false)
        {
            A2NetClient_Request a2NetClientRequest = A2NetClient_Request.Create();
            a2NetClientRequest.MessageObject = request;
            using A2NetClient_Response a2NetClientResponse = await self.Root().GetComponent<ProcessInnerSender>().Call(self.netClientActorId, a2NetClientRequest) as A2NetClient_Response;
            IResponse response = a2NetClientResponse.MessageObject;
                        
            if (response.Error == ErrorCore.ERR_MessageTimeout)
            {
                throw new RpcException(response.Error, $"Rpc error: request, 注意Actor消息超时，请注意查看是否死锁或者没有reply: {request}, response: {response}");
            }

            if (needException && ErrorCore.IsRpcNeedThrowException(response.Error))
            {
                throw new RpcException(response.Error, $"Rpc error: {request}, response: {response}");
            }
            return response;
        }

    }
}