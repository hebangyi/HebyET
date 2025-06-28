namespace ET.Server.Data;

[MessageClientHandler(SceneType.Battle)]
public class C2B_PlayerGetAllAOIWorldDataHandler: MessageClientHandler<BattleRole, C2B_PlayerGetAllAOIWorldData, B2C_PlayerGetAllAOIWorldData>
{
    protected override void Run(BattleRole battleRole, C2B_PlayerGetAllAOIWorldData request, B2C_PlayerGetAllAOIWorldData response)
    {
        Log.Info($"C2B_PlayerGetAllAOIWorldData");
    }
}