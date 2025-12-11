using System.Net;
using Authpb;

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
            root.AddComponent<CoroutineLockComponent>();
            root.AddComponent<MessageSender>();
            
            //// 战斗
            // 战斗数据逻辑管理器
            root.AddComponent<LogicWorldLogicManagerComponent>();
            root.AddComponent<BattleEventManagerComponent>();
            
            // 状态机加载类
            root.AddComponent<StateMachineManagerComponent>();
            
            
            // 业务相关
            root.AddComponent<BattleMatchComponent>();
            root.AddComponent<BattleWorldManagerComponent>();

            root.AddComponent<BattleRoleComponent>();
            root.AddComponent<GlobalClockComponent>();
            root.AddComponent<MongoDBComponent>();
            
            
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
