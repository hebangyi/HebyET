namespace ET.Server;


[Invoke((long)SceneType.RouterServer)]
public class FiberInit_RouterServer: AInvokeHandler<FiberInit, ETTask>
{
    public override async ETTask Handle(FiberInit args)
    {
        await ETTask.CompletedTask;
    }
}