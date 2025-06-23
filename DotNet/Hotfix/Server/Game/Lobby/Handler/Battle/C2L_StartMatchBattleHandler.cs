using System;

namespace ET.Server;

[MessageClientHandler(SceneType.Lobby)]
public class C2L_StartMatchBattleHandler: MessageClientHandler<LobbyRole, C2L_StartMatchBattle, L2C_StartMatchBattle>
{
    protected override void Run(LobbyRole lobbyRole, C2L_StartMatchBattle request, L2C_StartMatchBattle response)
    {
        var messageSender = lobbyRole.Root().GetComponent<MessageSender>();
        var battle = EtcdHelper.GetRandomNode(SceneType.Battle);

        if (battle == null)
        {
            response.Error = ErrorCode.NotFoundBattleNode;
            return;
        }
        
        SendMatch(lobbyRole.Root(), battle).Coroutine();
    }


    public async ETTask SendMatch(Scene scene, SceneNodeInfo nodeInfo)
    {
        var messageSender = scene.GetComponent<MessageSender>();
        var response = await messageSender.Call(nodeInfo.GetActorId(), L2B_PlayerStartMatch.Create()) as B2L_PlayerStartMatch;
    }
}