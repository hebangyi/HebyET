using System.Net;

namespace ET.Server
{
    [Event(SceneType.All)]
    public class FiberExit_InitServer: AEvent<Scene, FiberExit>
    {
        protected override async ETTask Run(Scene scene, FiberExit a)
        {
            var timerComponent = scene.GetComponent<TimerComponent>();
            
            Log.Info("协程开始退出...");
            if (timerComponent != null)
            {
                await timerComponent.WaitAsync(1000);
            }
            
            Log.Info("协程退出完成...");
            await ETTask.CompletedTask;
        }
    }


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
            root.AddComponent<LobbyRoleComponent>();
            root.AddComponent<GateSessionKeyComponent>();
            root.AddComponent<LocationProxyComponent>();
            root.AddComponent<MessageLocationSenderComponent>();
            root.AddComponent<MongoDBComponent>();
            root.AddComponent<MongoCacheAgentComponent>();
            root.AddComponent<GlobalClockComponent>();
            
            await EventSystem.Instance.PublishAsync(root, new InitGlobalComponentEvent {});
            
            await EventSystem.Instance.PublishAsync(root, new InitGlobalComponentFinishEvent {});
            
            // 对外暴露端口
            var netComponentConfig = ProcessConfig.Instance.GetSceneComponentConfig<NetComponentConfig>(fiberInit.Fiber.Root);
            var innerPort = new IPEndPoint(IPAddress.Any, netComponentConfig.OuterPort);
            root.AddComponent<NetComponent, IPEndPoint, NetworkProtocol>(innerPort, NetworkProtocol.UDP);
            await ETTask.CompletedTask;
        }
    }
}