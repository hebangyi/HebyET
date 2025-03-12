using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace ET.Client
{
    public static class LoginHelper
    {
        public static async ETTask Login(Scene root, string account, string password)
        {
            root.RemoveComponent<ClientSenderComponent>();
            
            ClientSenderComponent clientSenderComponent = root.AddComponent<ClientSenderComponent>();
            
            long playerId = await clientSenderComponent.LoginAsync(account, password);
            root.GetComponent<PlayerComponent>().MyId = playerId;
            
            // 同步全量数据
            C2G_GetAllDataUnits getAllDataUnits = C2G_GetAllDataUnits.Create();
            var response = (G2_GetAllDataUnits)await clientSenderComponent.Call(getAllDataUnits);
            
            if (response.Error == (int)ErrorCode.ERR_Success)
            {
                var unitStructData = response.UnitStructData;
                foreach (var unitByte in unitStructData.DataUnitBytes)
                {
                    var unitId = unitByte.UnitId;
                    Type unitDataType = OpcodeType.Instance.GetType((ushort)unitId);
                    // TODO
                    if (unitDataType == null)
                    {
                        
                    }

                    var converter = DataUnitManager.Instance.UnitDataType2Converters.GetValueOrDefault(unitDataType);
                    // TODO
                    if (converter == null)
                    {
                  
                    }
                    
                    var data = MemoryPackHelper.Deserialize(unitDataType, unitByte.UnitBytes, 0, unitByte.UnitBytes.Length);
                    Log.Info(data.ToJson());
                }
            }
            await EventSystem.Instance.PublishAsync(root, new LoginFinish());
        }
    }
}