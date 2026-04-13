using System;

namespace ET.Client
{
    [EntitySystemOf(typeof(ClientBattleSenderComponent))]
    [FriendOf(typeof(ClientBattleSenderComponent))]
    public static partial class ClientBattleSenderComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ClientBattleSenderComponent self)
        {
            ClientBattleSenderComponent.Instance = self;
            self.TimerContext = UpdateLogicManagerComponent.Instance.AddTaskUpdateFunc(self.ExecuteUpdate);
        }
        
        [EntitySystem]
        private static void Destroy(this ClientBattleSenderComponent self)
        {
            if (self.TimerContext != null)
            {
                self.TimerContext.IsRemove = true;
            }
            
            ClientBattleSenderComponent.Instance = null;
            self.RemoveFiberAsync().Coroutine();
        }
        
        public static async ETTask ExecuteUpdate(this ClientBattleSenderComponent self, UpdateLogicManagerComponent.TaskIntervalUpdateContext c)
        {
            foreach (var kv in self.Type2ClientMessage)
            {
                var context = kv.Value;
                if (context.IsSend)
                {
                    continue;
                }

                self.CallContext(context).Coroutine();
            }

            await ETTask.CompletedTask;
        }

        public static async ETTask CallContext(this ClientBattleSenderComponent self, ClientBattleQueueMessage context)
        {
            context.IsSend = true;
            var response = await self.Call(context.Request);
            context.Response.SetResult(response);

            // 移除
            self.Type2ClientMessage.Remove(context.RequestType);
        }
        
        
        public static async ETTask<int> SessionLogin(this ClientBattleSenderComponent self, string routerAddress, string address, string token)
        {
            self.fiberId = await FiberManager.Instance.Create(SchedulerType.ThreadPool, 0, SceneType.NetBattle, "");
            self.netClientActorId = new ActorId(self.Fiber().Process, self.fiberId);

            var request = Main2NetBattleLogin.Create();
            request.OwnerFiberId = self.Fiber().Id;
            request.Token = token;
            request.RouterAddress = routerAddress;
            request.Address = address;
            
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
        
        
        public static void Send(this ClientBattleSenderComponent self, IMessage message)
        {
            A2NetClient_Message a2NetClientMessage = A2NetClient_Message.Create();
            a2NetClientMessage.MessageObject = message;
            self.Root().GetComponent<ProcessInnerSender>().Send(self.netClientActorId, a2NetClientMessage);
        }
        
        
        private static async ETTask<IResponse> Call(this ClientBattleSenderComponent self, IRequest request)
        {
            A2NetClient_Request a2NetClientRequest = A2NetClient_Request.Create();
            a2NetClientRequest.MessageObject = request;

            using A2NetClient_Response a2NetClientResponse = await self.Root().GetComponent<ProcessInnerSender>().Call(self.netClientActorId, a2NetClientRequest) as A2NetClient_Response;
            IResponse response = a2NetClientResponse.MessageObject;
            
            if (response == null)
            {
                Type responseType = OpcodeType.Instance.GetResponseType(request.GetType());
                response = Activator.CreateInstance(responseType) as IResponse;
                response.Error = a2NetClientResponse.Error;
            }
            return response;
        }
    }
}
