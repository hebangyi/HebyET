using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET.Server;

[ChildOf(typeof(LobbyRoleComponent))]
public class LobbyRole : Entity, IAwake
{
    public long RoleId; // 玩家id
    public string NickName; // 昵称
    
    //// 内存状态
    public long LoginOutTime; // 上次登出的时间
    public LobbyRoleStatus RoleStatus = LobbyRoleStatus.InitDB; // 玩家状态
}

public class LobbyRoleEntity : MongoEntity
{
    
}

public enum LobbyRoleStatus
{
    InitDB = 0,
    Online = 1, // 在线
    OffOnline = 2, // 离线
    UnloadingDB = 3, // 正在存储DB
    UnloadDB = 4, //  已经存储DB
}