using System.Net;

namespace ET.Server
{
    [Invoke((long)SceneType.Lobby)]
    public class FiberInit_Lobby: AInvokeHandler<FiberInit, ETTask>
    {
        public override async ETTask Handle(FiberInit fiberInit)
        {
            Scene root = fiberInit.Fiber.Root;
            root.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.UnOrderedMessage);
            root.AddComponent<TimerComponent>();
            root.AddComponent<CoroutineLockComponent>();
            root.AddComponent<ProcessInnerSender>();
            root.AddComponent<MessageSender>();
            
            // 业务相关
            root.AddComponent<LobbyRoleComponent>();
            root.AddComponent<LocationProxyComponent>();
            root.AddComponent<MessageLocationSenderComponent>();
            root.AddComponent<MongoDBComponent>();
            root.AddComponent<MongoAutoSaveComponent>();
            root.AddComponent<GlobalClockComponent>();
            
            await EventSystem.Instance.PublishAsync(root, new InitServerEvent ());
            await EventSystem.Instance.PublishAsync(root, new InitServerFinishEvent ());
            
            // 对外暴露端口
            var netComponentConfig = ProcessConfig.Instance.GetSceneComponentConfig<NetComponentConfig>(fiberInit.Fiber.Root);
            var innerPort = new IPEndPoint(IPAddress.Any, netComponentConfig.OuterPort);
            root.AddComponent<NetComponent, IPEndPoint, NetworkProtocol>(innerPort, NetworkProtocol.UDP);
            await ETTask.CompletedTask;
        }
    }
}