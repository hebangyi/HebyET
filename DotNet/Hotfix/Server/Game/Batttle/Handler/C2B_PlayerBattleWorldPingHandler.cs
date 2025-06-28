namespace ET.Server;

[MessageClientHandler(SceneType.Battle)]
public class C2B_PlayerBattleWorldPingHandler: MessageClientHandler<BattleRole, C2B_PlayerBattleWorldPing, B2C_PlayerBattleWorldPing>
{
    protected override void Run(BattleRole battleRole, C2B_PlayerBattleWorldPing request, B2C_PlayerBattleWorldPing response)
    {
        
    }
}