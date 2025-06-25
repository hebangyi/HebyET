namespace ET.Server;

[MessageHandler(SceneType.Battle)]
public class L2B_PlayerStartMatchHandler: MessageHandler<Scene, L2B_PlayerStartMatch, B2L_PlayerStartMatch>
{
    protected override async ETTask Run(Scene scene, L2B_PlayerStartMatch request, B2L_PlayerStartMatch response)
    {
        Log.Info($"玩家 {request.PlayerId} 请求匹配...");
        if (BattleMatchComponent.Instance.HasMatchOrder(request.PlayerId))
        {
            response.Error = ErrorCode.PlayerIsMatching;
            return;
        }
        
        MatchOrder matchOrder = new();
        matchOrder.MatchTime = TimeInfo.Instance.NowSec();
        matchOrder.PlayerId = request.PlayerId;
        matchOrder.ActorId = request.ActorId;
        
        BattleMatchComponent.Instance.AddMatchOrder(matchOrder);
        await ETTask.CompletedTask;
    }
}