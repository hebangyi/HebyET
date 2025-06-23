namespace ET.Server;

[MessageHandler(SceneType.Location)]
public class L2B_PlayerStartMatchHandler: MessageHandler<Scene, L2B_PlayerStartMatch, B2L_PlayerStartMatch>
{
    protected override async ETTask Run(Scene scene, L2B_PlayerStartMatch request, B2L_PlayerStartMatch response)
    {
        Log.Info("开始进入匹配");
        await ETTask.CompletedTask;
    }
}