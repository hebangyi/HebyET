using System.Net;

namespace ET.Server
{
    [Invoke((long)SceneType.Router)]
    public class FiberInit_Router: AInvokeHandler<FiberInit, ETTask>
    {
        public override async ETTask Handle(FiberInit fiberInit)
        {
            Scene root = fiberInit.Fiber.Root;
            // 开发期间使用OuterIPPort，云服务器因为本机没有OuterIP，所以要改成InnerIPPort，然后在云防火墙中端口映射到InnerIPPort
            root.AddComponent<RouterComponent>();
            await ETTask.CompletedTask;
        }
    }
}