namespace ET.Server;

[MessageClientHandler(SceneType.Lobby)]
[FriendOf(typeof(RoleInfoComponent))]
public class C2G_GetAllDataUnitsHandler : MessageClientHandler<LobbyRole, C2G_GetAllDataUnits, G2_GetAllDataUnits>
{
    protected override void Run(LobbyRole lobbyRole, C2G_GetAllDataUnits request, G2_GetAllDataUnits response)
    {
        var structData = LobbySyncUnitDataHelper.GetAllData(lobbyRole);
        response.UnitStructData = structData;
    }
}