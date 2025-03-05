namespace ET.Server;

[MessageLocationHandler(SceneType.Lobby)]
public class C2G_GetAllDataUnitsHandler: MessageLocationHandler<LobbyRole, C2G_GetAllDataUnits, G2_GetAllDataUnits>
{
    protected override async ETTask Run(LobbyRole lobbyRole, C2G_GetAllDataUnits request, G2_GetAllDataUnits response)
    {
        await ETTask.CompletedTask;
        return;
    }
}