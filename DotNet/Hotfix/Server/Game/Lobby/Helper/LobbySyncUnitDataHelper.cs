using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ET.Server;
// 定时同步客户端
[Event(SceneType.Lobby)]
public class LobbyRoleOneSecEvent_SyncUnitClient: AEvent<Scene, LobbyRoleOneSecEvent>
{
    protected override async ETTask Run(Scene scene, LobbyRoleOneSecEvent args)
    {
        Log.Info("player 自动检查 flush 消息");
        args.LobbyRole.FlushDirtyMessage();
        await ETTask.CompletedTask;
    }
}


[FriendOf(typeof(LobbySyncUnitDataComponent))]
public static class LobbySyncUnitDataHelper
{
    // 立即同步客户端
    public static void AddDirtyImmediately(this LobbyRole lobbyRole, IServerData serverData)
    {
        _addDirty0(lobbyRole, serverData, true);
    }
    
    // 同步客户端 (加入缓存 1s后批量发送)
    public static void AddDirty(this LobbyRole lobbyRole, IServerData serverData)
    {
        _addDirty0(lobbyRole, serverData, false);
    }


    private static void _addDirty0(LobbyRole lobbyRole, IServerData serverData, bool immediate)
    {
        var lobbySyncUnitDataComponent = lobbyRole.GetComponent<LobbySyncUnitDataComponent>();
        if (lobbySyncUnitDataComponent == null)
        {
            return;
        }

        var type = serverData.GetType();
        lobbySyncUnitDataComponent.CacheDirtyData[type] = serverData;
        if (immediate)
        {
            lobbyRole.FlushDirtyMessage();
        }
    }
    

    public static SyncDataUnitStruct GetAllData(LobbyRole lobbyRole)
    {
        var lobbySyncUnitDataComponent = lobbyRole.GetComponent<LobbySyncUnitDataComponent>();
        SyncDataUnitStruct structData = SyncDataUnitStruct.Create();
        structData.Frame = lobbySyncUnitDataComponent.frame;
        
        foreach (var componentKv in lobbyRole.Components)
        {
            var componentIns = componentKv.Value;
            var componentType = componentKv.Value.GetType();
            var fieldInfos = componentType.GetFields().Where(field => field.GetCustomAttribute<MongoFieldAttribute>() != null).ToList();

            foreach (var fieldInfo in fieldInfos)
            {
                if (fieldInfo.GetValue(componentIns) is IServerData serverData)
                {
                    var unitBytes = ToDataUnitBytes(serverData);
                    if (unitBytes != null)
                    {
                        structData.DataUnitBytes.Add(unitBytes);    
                    }
                }
            }
        }
        
        return structData;
    }


    private static DataUnitBytes ToDataUnitBytes(IServerData serverData)
    {
        var serverDataType = serverData.GetType(); 
        
        var converter = DataUnitManager.Instance.ServerDataType2Converters.GetValueOrDefault(serverDataType);
        if (converter == null)
        {
            Log.Error($"{serverData.GetType().FullName} 转换 UnitData 失败 无法找到 converter");
            return null;
        }
        
        var unitData = converter.ToUnitData(serverData);
        if (unitData == null)
        {
            Log.Error($"{serverData.GetType().FullName} 转换 UnitData 失败 转换对象为 Null");
            return null;
        }

        DataUnitBytes dataUnitBytes = DataUnitBytes.Create();
        var unitId = OpcodeType.Instance.GetOpcode(unitData.GetType());
        if (unitId == 0)
        {
            Log.Error($"未定义类型 {unitData.GetType()} 的 unitId");
        }
            
        dataUnitBytes.UnitId = unitId;
            
        // TODO 使用对象池
        dataUnitBytes.UnitBytes = MemoryPackHelper.Serialize(unitData);
        return dataUnitBytes;
    } 
    
    public static void FlushDirtyMessage(this LobbyRole lobbyRole)
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
            var serverData = dirtyDataKv.Value;

            var unitBytes = ToDataUnitBytes(serverData);
            if (unitBytes != null)
            {
                structData.DataUnitBytes.Add(unitBytes);    
            }
        }

        lobbyRole.SendToClient(message);
        lobbySyncUnitDataComponent.CacheDirtyData.Clear();
    }
}