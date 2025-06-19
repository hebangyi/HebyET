using MemoryPack;
using System.Collections.Generic;

namespace ET
{
    // 战斗通讯实体
    // 常规信息
    [MemoryPackable]
    [Message(ClientMessage.UnitEntityInfo)]
    public partial class UnitEntityInfo : MessageObject, IUnitEntityElemData
    {
        public static UnitEntityInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(UnitEntityInfo), isFromPool) as UnitEntityInfo;
        }

        /// <summary>
        /// 实体类型
        /// </summary>
        [MemoryPackOrder(0)]
        public UnitEntityTypeEnum unitEntityTypeEnum { get; set; }

        /// <summary>
        /// 配置ID
        /// </summary>
        [MemoryPackOrder(1)]
        public int ConfigId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.unitEntityTypeEnum = default;
            this.ConfigId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    // 玩家信息
    [MemoryPackable]
    [Message(ClientMessage.UnitEntityPlayerInfo)]
    public partial class UnitEntityPlayerInfo : MessageObject, IUnitEntityElemData
    {
        public static UnitEntityPlayerInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(UnitEntityPlayerInfo), isFromPool) as UnitEntityPlayerInfo;
        }

        /// <summary>
        /// 玩家ID
        /// </summary>
        [MemoryPackOrder(0)]
        public long playerId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.playerId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    /// <summary>
    /// 客户端Main向网络线程发送消息
    /// </summary>
    [MemoryPackable]
    [Message(ClientMessage.Main2NetBattleLogin)]
    [ResponseType(nameof(NetBattle2MainLogin))]
    public partial class Main2NetBattleLogin : MessageObject, IRequest
    {
        public static Main2NetBattleLogin Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Main2NetBattleLogin), isFromPool) as Main2NetBattleLogin;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public string Token { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Token = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(ClientMessage.NetBattle2MainLogin)]
    public partial class NetBattle2MainLogin : MessageObject, IResponse
    {
        public static NetBattle2MainLogin Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(NetBattle2MainLogin), isFromPool) as NetBattle2MainLogin;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    // 3.玩家进入战斗 对Session 进行登录验证
    [MemoryPackable]
    [Message(ClientMessage.C2B_PlayerEnterBattle)]
    public partial class C2B_PlayerEnterBattle : MessageObject, ISessionRequest
    {
        public static C2B_PlayerEnterBattle Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2B_PlayerEnterBattle), isFromPool) as C2B_PlayerEnterBattle;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public string Token { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Token = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(ClientMessage.B2C_PlayerEnterBattle)]
    [ResponseType(nameof(B2C_PlayerEnterBattle))]
    public partial class B2C_PlayerEnterBattle : MessageObject, ISessionResponse
    {
        public static B2C_PlayerEnterBattle Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(B2C_PlayerEnterBattle), isFromPool) as B2C_PlayerEnterBattle;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    // 4.玩家通知准备完成
    [MemoryPackable]
    [Message(ClientMessage.C2B_PlayerReadyCompleted)]
    [ResponseType(nameof(B2C_PlayerReadyCompleted))]
    public partial class C2B_PlayerReadyCompleted : MessageObject
    {
        public static C2B_PlayerReadyCompleted Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2B_PlayerReadyCompleted), isFromPool) as C2B_PlayerReadyCompleted;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(ClientMessage.B2C_PlayerReadyCompleted)]
    public partial class B2C_PlayerReadyCompleted : MessageObject
    {
        public static B2C_PlayerReadyCompleted Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(B2C_PlayerReadyCompleted), isFromPool) as B2C_PlayerReadyCompleted;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    // 未登录的常规协议
    /// <summary>
    /// 网络
    /// </summary>
    [MemoryPackable]
    [Message(ClientMessage.C2G_Ping)]
    [ResponseType(nameof(G2C_Ping))]
    public partial class C2G_Ping : MessageObject, ISessionRequest
    {
        public static C2G_Ping Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2G_Ping), isFromPool) as C2G_Ping;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(ClientMessage.G2C_Ping)]
    public partial class G2C_Ping : MessageObject, ISessionResponse
    {
        public static G2C_Ping Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(G2C_Ping), isFromPool) as G2C_Ping;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public long Time { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.Time = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(ClientMessage.C2G_Benchmark)]
    [ResponseType(nameof(G2C_Benchmark))]
    public partial class C2G_Benchmark : MessageObject, ISessionRequest
    {
        public static C2G_Benchmark Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2G_Benchmark), isFromPool) as C2G_Benchmark;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(ClientMessage.G2C_Benchmark)]
    public partial class G2C_Benchmark : MessageObject, ISessionResponse
    {
        public static G2C_Benchmark Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(G2C_Benchmark), isFromPool) as G2C_Benchmark;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    /// <summary>
    /// 客户端Main向网络线程发送消息
    /// </summary>
    [MemoryPackable]
    [Message(ClientMessage.Main2NetLobbyLogin)]
    [ResponseType(nameof(NetLobby2MainLogin))]
    public partial class Main2NetLobbyLogin : MessageObject, IRequest
    {
        public static Main2NetLobbyLogin Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Main2NetLobbyLogin), isFromPool) as Main2NetLobbyLogin;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int OwnerFiberId { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        [MemoryPackOrder(2)]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [MemoryPackOrder(3)]
        public string Password { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.OwnerFiberId = default;
            this.Account = default;
            this.Password = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(ClientMessage.NetLobby2MainLogin)]
    public partial class NetLobby2MainLogin : MessageObject, IResponse
    {
        public static NetLobby2MainLogin Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(NetLobby2MainLogin), isFromPool) as NetLobby2MainLogin;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public long PlayerId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.PlayerId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    // 登录账号服务器
    [MemoryPackable]
    [Message(ClientMessage.C2A_Login)]
    [ResponseType(nameof(A2C_Login))]
    public partial class C2A_Login : MessageObject, ISessionRequest
    {
        public static C2A_Login Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2A_Login), isFromPool) as C2A_Login;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        /// <summary>
        /// 帐号
        /// </summary>
        [MemoryPackOrder(1)]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [MemoryPackOrder(2)]
        public string Password { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Account = default;
            this.Password = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(ClientMessage.A2C_Login)]
    public partial class A2C_Login : MessageObject, ISessionResponse
    {
        public static A2C_Login Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(A2C_Login), isFromPool) as A2C_Login;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        /// <summary>
        /// 大厅地址
        /// </summary>
        [MemoryPackOrder(3)]
        public string Address { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        [MemoryPackOrder(4)]
        public string Token { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.Address = default;
            this.Token = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    // 登录大厅服务器
    [MemoryPackable]
    [Message(ClientMessage.C2L_LoginLobby)]
    [ResponseType(nameof(L2C_LoginLobby))]
    public partial class C2L_LoginLobby : MessageObject, ISessionRequest
    {
        public static C2L_LoginLobby Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2L_LoginLobby), isFromPool) as C2L_LoginLobby;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        [MemoryPackOrder(1)]
        public string Token { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Token = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(ClientMessage.L2C_LoginLobby)]
    public partial class L2C_LoginLobby : MessageObject, ISessionResponse
    {
        public static L2C_LoginLobby Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(L2C_LoginLobby), isFromPool) as L2C_LoginLobby;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public long PlayerId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.PlayerId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    // 链接服务器主动断开 (不要重连)
    [MemoryPackable]
    [Message(ClientMessage.G2C_SessionDisconnect)]
    public partial class G2C_SessionDisconnect : MessageObject, IMessage
    {
        public static G2C_SessionDisconnect Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(G2C_SessionDisconnect), isFromPool) as G2C_SessionDisconnect;
        }

        /// <summary>
        /// 断开原因
        /// </summary>
        [MemoryPackOrder(0)]
        public int Error { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.Error = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(ClientMessage.HttpGetRouterResponse)]
    public partial class HttpGetRouterResponse : MessageObject
    {
        public static HttpGetRouterResponse Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(HttpGetRouterResponse), isFromPool) as HttpGetRouterResponse;
        }

        [MemoryPackOrder(0)]
        public List<string> Routers { get; set; } = new();

        [MemoryPackOrder(1)]
        public List<string> Accounts { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.Routers.Clear();
            this.Accounts.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    // 玩家与大厅间的数据同步接口
    // ============================= 数据结构 =============================
    // 客户端服务器通用同步
    [MemoryPackable]
    [Message(ClientMessage.SyncDataUnitStruct)]
    public partial class SyncDataUnitStruct : MessageObject
    {
        public static SyncDataUnitStruct Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(SyncDataUnitStruct), isFromPool) as SyncDataUnitStruct;
        }

        /// <summary>
        /// 编号
        /// </summary>
        [MemoryPackOrder(0)]
        public uint Frame { get; set; }

        /// <summary>
        /// 数据单元
        /// </summary>
        [MemoryPackOrder(1)]
        public List<DataUnitBytes> DataUnitBytes { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.Frame = default;
            this.DataUnitBytes.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    // 数据单元
    [MemoryPackable]
    [Message(ClientMessage.DataUnitBytes)]
    public partial class DataUnitBytes : MessageObject
    {
        public static DataUnitBytes Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(DataUnitBytes), isFromPool) as DataUnitBytes;
        }

        /// <summary>
        /// 单位uintId
        /// </summary>
        [MemoryPackOrder(0)]
        public uint UnitId { get; set; }

        /// <summary>
        /// 单位数据
        /// </summary>
        [MemoryPackOrder(1)]
        public byte[] UnitBytes { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.UnitId = default;
            this.UnitBytes = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    // ============================= 协议 =============================
    /// <summary>
    /// 数据同步
    /// </summary>
    // 获得所有的 DataUnit 数据
    [MemoryPackable]
    [Message(ClientMessage.C2G_GetAllDataUnits)]
    [ResponseType(nameof(G2_GetAllDataUnits))]
    public partial class C2G_GetAllDataUnits : MessageObject, IClientRequest
    {
        public static C2G_GetAllDataUnits Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2G_GetAllDataUnits), isFromPool) as C2G_GetAllDataUnits;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(ClientMessage.G2_GetAllDataUnits)]
    public partial class G2_GetAllDataUnits : MessageObject, IClientResponse
    {
        public static G2_GetAllDataUnits Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(G2_GetAllDataUnits), isFromPool) as G2_GetAllDataUnits;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        [MemoryPackOrder(0)]
        public SyncDataUnitStruct UnitStructData { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.UnitStructData = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(ClientMessage.L2C_SyncDirtyDataUnits)]
    public partial class L2C_SyncDirtyDataUnits : MessageObject, IMessage
    {
        public static L2C_SyncDirtyDataUnits Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(L2C_SyncDirtyDataUnits), isFromPool) as L2C_SyncDirtyDataUnits;
        }

        /// <summary>
        /// 数据
        /// </summary>
        [MemoryPackOrder(0)]
        public SyncDataUnitStruct UnitStructData { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.UnitStructData = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    // ============================= 数据 =============================
    /// <summary>
    /// 玩家 角色信息
    /// </summary>
    [MemoryPackable]
    [Message(ClientMessage.RoleInfoUnitData)]
    public partial class RoleInfoUnitData : MessageObject, IUnitData
    {
        public static RoleInfoUnitData Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(RoleInfoUnitData), isFromPool) as RoleInfoUnitData;
        }

        /// <summary>
        /// 昵称
        /// </summary>
        [MemoryPackOrder(0)]
        public string NickName { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.NickName = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    /// <summary>
    /// 战斗流程
    /// </summary>
    // 1.开始匹配战斗
    [MemoryPackable]
    [Message(ClientMessage.C2L_StartMatchBattle)]
    [ResponseType(nameof(L2C_StartMatchBattle))]
    public partial class C2L_StartMatchBattle : MessageObject, IClientRequest
    {
        public static C2L_StartMatchBattle Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2L_StartMatchBattle), isFromPool) as C2L_StartMatchBattle;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        /// <summary>
        /// TODO 各个战斗服的ping值
        /// </summary>
        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(ClientMessage.L2C_StartMatchBattle)]
    public partial class L2C_StartMatchBattle : MessageObject, IClientResponse
    {
        public static L2C_StartMatchBattle Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(L2C_StartMatchBattle), isFromPool) as L2C_StartMatchBattle;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    // 2.通知匹配成功
    [MemoryPackable]
    [Message(ClientMessage.L2C_MatchBattleSuccess)]
    public partial class L2C_MatchBattleSuccess : MessageObject, IMessage
    {
        public static L2C_MatchBattleSuccess Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(L2C_MatchBattleSuccess), isFromPool) as L2C_MatchBattleSuccess;
        }

        /// <summary>
        /// 匹配的服务器 Node
        /// </summary>
        /// <summary>
        /// TODO 服务器 Node
        /// </summary>
        /// <summary>
        /// 加入玩家的签名
        /// </summary>
        [MemoryPackOrder(0)]
        public string Token { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.Token = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    public static class ClientMessage
    {
        public const ushort UnitEntityInfo = 10001;
        public const ushort UnitEntityPlayerInfo = 10002;
        public const ushort Main2NetBattleLogin = 10003;
        public const ushort NetBattle2MainLogin = 10004;
        public const ushort C2B_PlayerEnterBattle = 10005;
        public const ushort B2C_PlayerEnterBattle = 10006;
        public const ushort C2B_PlayerReadyCompleted = 10007;
        public const ushort B2C_PlayerReadyCompleted = 10008;
        public const ushort C2G_Ping = 10009;
        public const ushort G2C_Ping = 10010;
        public const ushort C2G_Benchmark = 10011;
        public const ushort G2C_Benchmark = 10012;
        public const ushort Main2NetLobbyLogin = 10013;
        public const ushort NetLobby2MainLogin = 10014;
        public const ushort C2A_Login = 10015;
        public const ushort A2C_Login = 10016;
        public const ushort C2L_LoginLobby = 10017;
        public const ushort L2C_LoginLobby = 10018;
        public const ushort G2C_SessionDisconnect = 10019;
        public const ushort HttpGetRouterResponse = 10020;
        public const ushort SyncDataUnitStruct = 10021;
        public const ushort DataUnitBytes = 10022;
        public const ushort C2G_GetAllDataUnits = 10023;
        public const ushort G2_GetAllDataUnits = 10024;
        public const ushort L2C_SyncDirtyDataUnits = 10025;
        public const ushort RoleInfoUnitData = 10026;
        public const ushort C2L_StartMatchBattle = 10027;
        public const ushort L2C_StartMatchBattle = 10028;
        public const ushort L2C_MatchBattleSuccess = 10029;
    }
}