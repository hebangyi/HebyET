using System.Net;

namespace ET.Server
{
    [Invoke((long)SceneType.Battle)]
    public class FiberInit_Battle: AInvokeHandler<FiberInit, ETTask>
    {
        public override async ETTask Handle(FiberInit fiberInit)
        {
            Scene root = fiberInit.Fiber.Root;
            root.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.UnOrderedMessage);
            root.AddComponent<TimerComponent>();
            root.AddComponent<ProcessInnerSender>();
            
            
            await EventSystem.Instance.PublishAsync(root, new InitServerEvent ());
            await EventSystem.Instance.PublishAsync(root, new InitServerFinishEvent ());
            await ETTask.CompletedTask;
            
            // 对外暴露端口
            var netComponentConfig = ProcessConfig.Instance.GetSceneComponentConfig<NetComponentConfig>(fiberInit.Fiber.Root);
            
            var innerPort = new IPEndPoint(IPAddress.Any, netComponentConfig.OuterPort);
            root.AddComponent<NetComponent, IPEndPoint, NetworkProtocol>(innerPort, NetworkProtocol.UDP);
            await ETTask.CompletedTask;
        }
    }    
}
