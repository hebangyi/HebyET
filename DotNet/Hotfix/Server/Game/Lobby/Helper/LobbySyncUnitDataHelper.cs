using System.Collections.Generic;

namespace ET.Server;

[FriendOf(typeof(LobbySyncUnitDataComponent))]
public static class LobbySyncUnitDataHelper
{
    public static void AddDirty(this LobbyRole lobbyRole, object data)
    {
        var lobbySyncUnitDataComponent = lobbyRole.GetComponent<LobbySyncUnitDataComponent>();
        if (lobbySyncUnitDataComponent == null)
        {
            return;
        }

        var type = data.GetType();
        lobbySyncUnitDataComponent.CacheDirtyData[type] = data;
    }

    public static void SyncDirtyMessage(this LobbyRole lobbyRole)
    {
        var lobbySyncUnitDataComponent = lobbyRole.GetComponent<LobbySyncUnitDataComponent>();
        if (lobbySyncUnitDataComponent == null)
        {
            return;
        }

        if (lobbySyncUnitDataComponent.CacheDirtyData.Count == 0)
        {
            return;
        }

        L2C_SyncDirtyDataUnits message = new();
        var frame = ++lobbySyncUnitDataComponent.frame;
        SyncDataUnitStruct structData = SyncDataUnitStruct.Create();
        structData.Frame = frame;

        foreach (var dirtyDataKv in lobbySyncUnitDataComponent.CacheDirtyData)
        {
            var dataUnitType = dirtyDataKv.Key;
            var dataUnit = dirtyDataKv.Value;
            var iUnitData = DataUnitManager.Instance.ToUnitData(dataUnit);
            if (iUnitData == null)
            {
                Log.Error($"{dirtyDataKv.Key.FullName} 转换 UnitData 失败");
                continue;
            }

            DataUnitBytes dataUnitBytes = DataUnitBytes.Create();
            var unitId = OpcodeType.Instance.GetOpcode(dataUnitType);
            dataUnitBytes.UnitId = unitId;
            // TODO 使用对象池
            dataUnitBytes.UnitBytes = MemoryPackHelper.Serialize(iUnitData);
            structData.DataUnitBytes.Add(dataUnitBytes);
        }

        lobbyRole.SendToClient(message);
    }
}