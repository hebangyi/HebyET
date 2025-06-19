namespace ET.Client
{
    [Invoke((long)SceneType.NetLobby)]
    public class FiberInit_NetLobby: AInvokeHandler<FiberInit, ETTask>
    {
        public override async ETTask Handle(FiberInit fiberInit)
        {
            Scene root = fiberInit.Fiber.Root;
            root.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.UnOrderedMessage);
            root.AddComponent<TimerComponent>();
            root.AddComponent<CoroutineLockComponent>();
            root.AddComponent<ProcessInnerSender>();
            root.AddComponent<FiberParentComponent>();
            await ETTask.CompletedTask;
        }
    }
}