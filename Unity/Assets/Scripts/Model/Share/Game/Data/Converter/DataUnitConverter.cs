using System;
using System.Collections.Generic;

namespace ET
{
    public interface IClientData
    {
    }

    public interface IServerData
    {
    }

    public interface IUnitDataConverter
    {
        Type GetServerDataType();
        Type GetClientDataType();
        Type GetUnitDataType();
        IUnitData ToUnitData(IServerData data);
        IClientData FromUnitData(IUnitData unitData);
    }


    [DataUnitConverter]
    public abstract class UnitDataConverter<ServerData, ClientData, UnitData> : IUnitDataConverter where UnitData: MessageObject, IUnitData where ServerData: IServerData where ClientData : IClientData
    {

        public abstract UnitData ToUnitData(ServerData data);

        public abstract ClientData FromUnitData(UnitData unitData);

        
        public IUnitData ToUnitData(IServerData data)
        {
            if (data is not ServerData serverData)
            {
                Log.Error($"Server Data 转换 错误 转换器类型 {typeof(ServerData).FullName} 目标对象类型 {data.GetType().FullName}");
                return null;
            }

            return ToUnitData(serverData);
        }

        public IClientData FromUnitData(IUnitData data)
        {
            if (data is not UnitData unitData)
            {
                Log.Error($"UnitData Data 转换 错误 转换器类型 {typeof(UnitData).FullName} 目标对象类型 {data.GetType().FullName}");
                return null;
            }

            return FromUnitData(unitData);
        }
        
        public Type GetServerDataType()
        {
            return typeof (ServerData);
        }
    
        public Type GetClientDataType()
        {
            return typeof (ClientData);
        }
        
        public Type GetUnitDataType()
        {
            return typeof (UnitData);
        }


    }
}
