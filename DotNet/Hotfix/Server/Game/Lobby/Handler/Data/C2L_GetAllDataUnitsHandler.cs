namespace ET.Server;

[MessageClientHandler(SceneType.Lobby)]
[FriendOf(typeof(RoleInfoComponent))]
public class C2L_GetAllDataUnitsHandler : MessageClientHandler<LobbyRole, C2L_GetAllDataUnits, L2C_GetAllDataUnits>
{
    protected override void Run(LobbyRole lobbyRole, C2L_GetAllDataUnits request, L2C_GetAllDataUnits response)
    {
        var structData = LobbySyncUnitDataHelper.GetAllData(lobbyRole);
        response.UnitStructData = structData;
    }
}