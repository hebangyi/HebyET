namespace ET.Server;

[MessageClientHandler(SceneType.Lobby)]
public class C2L_StartMatchBattleHandler: MessageClientHandler<LobbyRole, C2L_StartMatchBattle, L2C_StartMatchBattle>
{
    protected override void Run(LobbyRole e, C2L_StartMatchBattle request, L2C_StartMatchBattle response)
    {
        
    }
}