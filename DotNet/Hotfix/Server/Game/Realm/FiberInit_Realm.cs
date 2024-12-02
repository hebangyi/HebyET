using System.Net;

namespace ET.Server
{
    [Invoke((long)SceneType.Account)]
    public class FiberInit_Realm: AInvokeHandler<FiberInit, ETTask>
    {
        public override async ETTask Handle(FiberInit fiberInit)
        {
            Scene root = fiberInit.Fiber.Root;
            root.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.UnOrderedMessage);
            root.AddComponent<TimerComponent>();
            root.AddComponent<CoroutineLockComponent>();
            root.AddComponent<ProcessInnerSender>();
            root.AddComponent<MessageSender>();
            
            var netComponentConfig = ProcessConfig.Instance.GetSceneComponentConfig<NetComponentConfig>(fiberInit.Fiber.Root);
            var innerPort = new IPEndPoint(IPAddress.Any, netComponentConfig.OuterPort);
            root.AddComponent<NetComponent, IPEndPoint, NetworkProtocol>(innerPort, NetworkProtocol.UDP);
            await ETTask.CompletedTask;
        }
    }
}