namespace ET.Server;

[MessageClientHandler(SceneType.Lobby)]
[FriendOf(typeof(RoleInfoComponent))]
public class C2G_GetAllDataUnitsHandler : MessageClientHandler<LobbyRole, C2G_GetAllDataUnits, G2_GetAllDataUnits>
{
    protected override void Run(LobbyRole lobbyRole, C2G_GetAllDataUnits request, G2_GetAllDataUnits response)
    {
        var roleInfoComponent = lobbyRole.GetComponent<RoleInfoComponent>();
        var iUnitData = DataUnitManager.Instance.ToUnitData(roleInfoComponent.roleInfoData);

        SyncDataUnitStruct structData = SyncDataUnitStruct.Create();
        
        var unitId = OpcodeType.Instance.GetOpcode(roleInfoComponent.roleInfoData.GetType());
        
        DataUnitBytes dataUnitBytes = DataUnitBytes.Create();
        dataUnitBytes.UnitId = unitId;
        dataUnitBytes.UnitBytes = MemoryPackHelper.Serialize(iUnitData);
        
        structData.DataUnitBytes.Add(dataUnitBytes);
        response.UnitStructData = structData;
    }
}