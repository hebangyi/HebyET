namespace ET.Server;

[MessageHandler(SceneType.Lobby)]
public class L2B_PlayerMatchSuccessNotifyHandler: MessageHandler<LobbyRole, L2B_PlayerMatchSuccessNotify>
{
    protected override async ETTask Run(LobbyRole lobbyRole, L2B_PlayerMatchSuccessNotify message)
    {
        var lobbyRoleBattleComponent = lobbyRole.TryAddComponent<LobbyRoleBattleComponent>();
        lobbyRoleBattleComponent.WorldId = message.worldId;

        BattleLoginRSA rsa = new ();
        rsa.RoleId = lobbyRole.RoleId;
        rsa.WorldId = message.worldId;
        
        var token = RSATokenManager.Instance.MakeToken(rsa);
        Log.Info($"通知玩家 {lobbyRole.RoleId} 匹配成功, 世界ID :{message.worldId} token {token}");
        
        // TODO 使用
        var battleNode = EtcdHelper.GetRandomNode(SceneType.Battle);
        // TODO 战斗服的Gate
        var routerGateNode = EtcdHelper.GetRandomNode(SceneType.RouterGate);
        
        
        if (battleNode != null && routerGateNode != null)
        {
            var clientMessage = L2C_MatchBattleSuccess.Create();
            clientMessage.RouterAddress = routerGateNode.InnerIpAndOuterPortAddress;
            clientMessage.BattleAddress = battleNode.InnerIpAndOuterPortAddress;
            clientMessage.Token = token;
            lobbyRole.SendToClient(clientMessage);
        }
        await ETTask.CompletedTask;
    }
}