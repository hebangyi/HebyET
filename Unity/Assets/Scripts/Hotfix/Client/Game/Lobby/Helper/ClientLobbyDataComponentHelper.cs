using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace ET.Client
{
    public static class ClientLobbyDataComponentHelper
    {
        public static void SyncDirtyData(Scene scene, SyncDataUnitStruct syncDirtyData)
        {
            var clientLobbyDataComponent = scene.GetComponent<ClientLobbyDataComponent>();
            if (clientLobbyDataComponent == null)
            {
                return;
            }

            foreach (var unitByte in syncDirtyData.DataUnitBytes)
            {
                var unitId = unitByte.UnitId;
                Type unitDataType = OpcodeType.Instance.GetType((ushort)unitId);
                if (unitDataType == null)
                {
                    Log.Error($"同步数据异常 UnitId {unitId} 无法找到 unit 类型");
                    continue;
                }

                var converter = DataUnitManager.Instance.UnitDataType2Converters.GetValueOrDefault(unitDataType);
                if (converter == null)
                {
                    Log.Error($"同步数据异常 UnityType {unitDataType.Name} 无法找到对应的客户端数据转化 Converter!");
                    continue;
                }

                var data = MemoryPackHelper.Deserialize(unitDataType, unitByte.UnitBytes, 0, unitByte.UnitBytes.Length) as IUnitData;

                var clientDataType = converter.GetClientDataType();
                var clientData = converter.FromUnitData(data);
                // 更新数据
                clientLobbyDataComponent.ClientData[clientDataType] = clientData;
                Log.Info($"更新数据 类型 {clientDataType.Name} 数据 {JsonHelper.ToJson(clientData)}");
            }
        }

        public static async ETTask<int> SyncAllData(Scene scene)
        {
            var clientSenderComponent = scene.GetComponent<ClientSenderComponent>();
            var clientLobbyDataComponent = scene.GetComponent<ClientLobbyDataComponent>();
            if (clientSenderComponent == null || clientLobbyDataComponent == null)
            {
                return ErrorCode.ClientInternalErr;
            }

            // 同步全量数据
            Log.Info("开始同步全量数据 ..");
            C2G_GetAllDataUnits getAllDataUnits = C2G_GetAllDataUnits.Create();
            var response = (G2_GetAllDataUnits)await clientSenderComponent.Call(getAllDataUnits);

            if (response.Error != (int)ErrorCode.ERR_Success)
            {
                return response.Error;
            }

            var unitStructData = response.UnitStructData;
            SyncDirtyData(scene, unitStructData);
            return ErrorCode.ERR_Success;
        }
    }
}