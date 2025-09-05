using MemoryPack;
using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 战斗世界通用数据结构
    /// </summary>
    [MemoryPackable]
    [Message(ClientMessage.BattleWorld)]
    public partial class BattleWorld : MessageObject
    {
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

        public static BattleWorld Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(BattleWorld), isFromPool) as BattleWorld;
        }

        /// <summary>
        /// 世界帧
        /// </summary>
        [MemoryPackOrder(0)]
        public uint Frame { get; set; }

        /// <summary>
        /// 世界状态
        /// </summary>
        [MemoryPackOrder(1)]
        public WorldStatusEnum WorldStatus { get; set; }

        /// <summary>
        /// TODO 世界的其他配置 天空盒 场景 等战场常规信息
        /// </summary>
        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.Frame = default;
            this.WorldStatus = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(ClientMessage.BattleUnitEntity)]
    public partial class BattleUnitEntity : MessageObject
    {
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

        public static BattleUnitEntity Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(BattleUnitEntity), isFromPool) as BattleUnitEntity;
        }

        /// <summary>
        /// Unit 实例id
        /// </summary>
        [MemoryPackOrder(0)]
        public long InsId { get; set; }

        /// <summary>
        /// 数据项
        /// </summary>
        [MemoryPackOrder(1)]
        public List<UnitEntityElemData> EleDatas { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.InsId = default;
            this.EleDatas.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(ClientMessage.UnitEntityElemData)]
    public partial class UnitEntityElemData : MessageObject
    {
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

        public static UnitEntityElemData Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(UnitEntityElemData), isFromPool) as UnitEntityElemData;
        }

        /// <summary>
        /// 组件id
        /// </summary>
        [MemoryPackOrder(0)]
        public ushort CompId { get; set; }

        /// <summary>
        /// element数据
        /// </summary>
        [MemoryPackOrder(1)]
        public byte[] ElemDatas { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.CompId = default;
            this.ElemDatas = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    /// <summary>
    /// >>>>>>>>>>>>>>>>>> 常规信息
    /// </summary>
    [MemoryPackable]
    [Message(ClientMessage.UnitEntityCommonData)]
    public partial class UnitEntityCommonData : MessageObject, IUnitEntityElemData
    {
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

        public static UnitEntityCommonData Create(long instanceId, IDirtyHandler dirtyHandler, bool isFromPool = false)
        {
            var instance = ObjectPool.Instance.Fetch(typeof(UnitEntityCommonData), isFromPool) as UnitEntityCommonData;
            instance.m_DirtyHandler = dirtyHandler;
            instance.m_InstanceId = instanceId;
            return instance;
        }

        private UnitEntityTypeEnum _UnitEntityType;

        [MemoryPackOrder(0)]
        public UnitEntityTypeEnum UnitEntityType
        {
            get => _UnitEntityType;
            set {
                _UnitEntityType = value;
                this.m_DirtyHandler?.Dirty(m_InstanceId, this);
            }
        }
        private Dictionary<string, string> _Datas = new();

        [MongoDB.Bson.Serialization.Attributes.BsonDictionaryOptions(MongoDB.Bson.Serialization.Options.DictionaryRepresentation.ArrayOfArrays)]
        [MemoryPackOrder(1)]
        public Dictionary<string, string> Datas 
        {
            get => _Datas;
            set {
                _Datas = value;
                this.m_DirtyHandler?.Dirty(m_InstanceId, this);
            }
        }
        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.m_DirtyHandler = null;
            this.m_InstanceId = default;
            
this._UnitEntityType = default;
            this._Datas.Clear();
            

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(ClientMessage.UnitEntityPosition)]
    public partial class UnitEntityPosition : MessageObject, IUnitEntityElemData
    {
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

        public static UnitEntityPosition Create(long instanceId, IDirtyHandler dirtyHandler, bool isFromPool = false)
        {
            var instance = ObjectPool.Instance.Fetch(typeof(UnitEntityPosition), isFromPool) as UnitEntityPosition;
            instance.m_DirtyHandler = dirtyHandler;
            instance.m_InstanceId = instanceId;
            return instance;
        }

        private Unity.Mathematics.float2 _Position;

        [MemoryPackOrder(0)]
        public Unity.Mathematics.float2 Position
        {
            get => _Position;
            set {
                _Position = value;
                this.m_DirtyHandler?.Dirty(m_InstanceId, this);
            }
        }
        private Unity.Mathematics.float3 _Forward;

        [MemoryPackOrder(1)]
        public Unity.Mathematics.float3 Forward
        {
            get => _Forward;
            set {
                _Forward = value;
                this.m_DirtyHandler?.Dirty(m_InstanceId, this);
            }
        }
        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.m_DirtyHandler = null;
            this.m_InstanceId = default;
            
this._Position = default;
            this._Forward = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    // 常规信息
    [MemoryPackable]
    [Message(ClientMessage.UnitEntityInfo)]
    public partial class UnitEntityInfo : MessageObject, IUnitEntityElemData
    {
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

        public static UnitEntityInfo Create(long instanceId, IDirtyHandler dirtyHandler, bool isFromPool = false)
        {
            var instance = ObjectPool.Instance.Fetch(typeof(UnitEntityInfo), isFromPool) as UnitEntityInfo;
            instance.m_DirtyHandler = dirtyHandler;
            instance.m_InstanceId = instanceId;
            return instance;
        }

        /// <summary>
        /// 实体类型
        /// </summary>
        private UnitEntityTypeEnum _UnitEntityTypeEnum;

        [MemoryPackOrder(0)]
        public UnitEntityTypeEnum UnitEntityTypeEnum
        {
            get => _UnitEntityTypeEnum;
            set {
                _UnitEntityTypeEnum = value;
                this.m_DirtyHandler?.Dirty(m_InstanceId, this);
            }
        }
        /// <summary>
        /// 配置ID
        /// </summary>
        private int _ConfigId;

        [MemoryPackOrder(1)]
        public int ConfigId
        {
            get => _ConfigId;
            set {
                _ConfigId = value;
                this.m_DirtyHandler?.Dirty(m_InstanceId, this);
            }
        }
        /// <summary>
        /// 移动速度
        /// </summary>
        private int _Speed;

        [MemoryPackOrder(2)]
        public int Speed
        {
            get => _Speed;
            set {
                _Speed = value;
                this.m_DirtyHandler?.Dirty(m_InstanceId, this);
            }
        }
        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.m_DirtyHandler = null;
            this.m_InstanceId = default;
            
this._UnitEntityTypeEnum = default;
            this._ConfigId = default;
            this._Speed = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    /// <summary>
    /// >>>>>>>>>>>>>>>>>>
    /// </summary>
    /// <summary>
    /// 玩家
    /// </summary>
    // 玩家信息
    [MemoryPackable]
    [Message(ClientMessage.UnitEntityPlayerInfo)]
    public partial class UnitEntityPlayerInfo : MessageObject, IUnitEntityElemData
    {
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

        public static UnitEntityPlayerInfo Create(long instanceId, IDirtyHandler dirtyHandler, bool isFromPool = false)
        {
            var instance = ObjectPool.Instance.Fetch(typeof(UnitEntityPlayerInfo), isFromPool) as UnitEntityPlayerInfo;
            instance.m_DirtyHandler = dirtyHandler;
            instance.m_InstanceId = instanceId;
            return instance;
        }

        /// <summary>
        /// 玩家ID
        /// </summary>
        private long _PlayerId;

        [MemoryPackOrder(0)]
        public long PlayerId
        {
            get => _PlayerId;
            set {
                _PlayerId = value;
                this.m_DirtyHandler?.Dirty(m_InstanceId, this);
            }
        }
        /// <summary>
        /// 是否在线
        /// </summary>
        private bool _IsOnline;

        [MemoryPackOrder(1)]
        public bool IsOnline
        {
            get => _IsOnline;
            set {
                _IsOnline = value;
                this.m_DirtyHandler?.Dirty(m_InstanceId, this);
            }
        }
        /// <summary>
        /// 上次登录时间
        /// </summary>
        private long _LastLoginTime;

        [MemoryPackOrder(2)]
        public long LastLoginTime
        {
            get => _LastLoginTime;
            set {
                _LastLoginTime = value;
                this.m_DirtyHandler?.Dirty(m_InstanceId, this);
            }
        }
        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.m_DirtyHandler = null;
            this.m_InstanceId = default;
            
this._PlayerId = default;
            this._IsOnline = default;
            this._LastLoginTime = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    // 玩家帧信息
    [MemoryPackable]
    [Message(ClientMessage.UnitEntityPlayerFrame)]
    public partial class UnitEntityPlayerFrame : MessageObject, IUnitEntityElemData
    {
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

        public static UnitEntityPlayerFrame Create(long instanceId, IDirtyHandler dirtyHandler, bool isFromPool = false)
        {
            var instance = ObjectPool.Instance.Fetch(typeof(UnitEntityPlayerFrame), isFromPool) as UnitEntityPlayerFrame;
            instance.m_DirtyHandler = dirtyHandler;
            instance.m_InstanceId = instanceId;
            return instance;
        }

        /// <summary>
        /// 当前玩家的帧率
        /// </summary>
        private uint _Frame;

        [MemoryPackOrder(0)]
        public uint Frame
        {
            get => _Frame;
            set {
                _Frame = value;
                this.m_DirtyHandler?.Dirty(m_InstanceId, this);
            }
        }
        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.m_DirtyHandler = null;
            this.m_InstanceId = default;
            
this._Frame = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    // 玩家操作
    [MemoryPackable]
    [Message(ClientMessage.UnitEntityPlayerOperationAction)]
    public partial class UnitEntityPlayerOperationAction : MessageObject, IUnitEntityElemData
    {
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

        public static UnitEntityPlayerOperationAction Create(long instanceId, IDirtyHandler dirtyHandler, bool isFromPool = false)
        {
            var instance = ObjectPool.Instance.Fetch(typeof(UnitEntityPlayerOperationAction), isFromPool) as UnitEntityPlayerOperationAction;
            instance.m_DirtyHandler = dirtyHandler;
            instance.m_InstanceId = instanceId;
            return instance;
        }

        /// <summary>
        /// 操作角度
        /// </summary>
        private short _MoveAngle;

        [MemoryPackOrder(0)]
        public short MoveAngle
        {
            get => _MoveAngle;
            set {
                _MoveAngle = value;
                this.m_DirtyHandler?.Dirty(m_InstanceId, this);
            }
        }
        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.m_DirtyHandler = null;
            this.m_InstanceId = default;
            
this._MoveAngle = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    /// <summary>
    /// 地块
    /// </summary>
    // 地图信息
    [MemoryPackable]
    [Message(ClientMessage.UnitEntityMapMessage)]
    public partial class UnitEntityMapMessage : MessageObject, IUnitEntityElemData
    {
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

        public static UnitEntityMapMessage Create(long instanceId, IDirtyHandler dirtyHandler, bool isFromPool = false)
        {
            var instance = ObjectPool.Instance.Fetch(typeof(UnitEntityMapMessage), isFromPool) as UnitEntityMapMessage;
            instance.m_DirtyHandler = dirtyHandler;
            instance.m_InstanceId = instanceId;
            return instance;
        }

        /// <summary>
        /// 地图数据
        /// </summary>
        private List<bool> _MapData = new();

        [MemoryPackOrder(0)]
        public List<bool> MapData
        {
            get => _MapData;
            set {
                _MapData = value;
                this.m_DirtyHandler?.Dirty(m_InstanceId, this);
            }
        }
        /// <summary>
        /// 地图宽
        /// </summary>
        private int _MapWidth;

        [MemoryPackOrder(1)]
        public int MapWidth
        {
            get => _MapWidth;
            set {
                _MapWidth = value;
                this.m_DirtyHandler?.Dirty(m_InstanceId, this);
            }
        }
        /// <summary>
        /// 地图高
        /// </summary>
        private int _MapHeight;

        [MemoryPackOrder(2)]
        public int MapHeight
        {
            get => _MapHeight;
            set {
                _MapHeight = value;
                this.m_DirtyHandler?.Dirty(m_InstanceId, this);
            }
        }
        /// <summary>
        /// 单位Cell的长度
        /// </summary>
        private int _UnitCellSize;

        [MemoryPackOrder(3)]
        public int UnitCellSize
        {
            get => _UnitCellSize;
            set {
                _UnitCellSize = value;
                this.m_DirtyHandler?.Dirty(m_InstanceId, this);
            }
        }
        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.m_DirtyHandler = null;
            this.m_InstanceId = default;
            
this._MapData.Clear();
            this._MapWidth = default;
            this._MapHeight = default;
            this._UnitCellSize = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    // 地块辅助线信息
    [MemoryPackable]
    [Message(ClientMessage.GizmosDebugInfo)]
    public partial class GizmosDebugInfo : MessageObject, IUnitEntityElemData
    {
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

        public static GizmosDebugInfo Create(long instanceId, IDirtyHandler dirtyHandler, bool isFromPool = false)
        {
            var instance = ObjectPool.Instance.Fetch(typeof(GizmosDebugInfo), isFromPool) as GizmosDebugInfo;
            instance.m_DirtyHandler = dirtyHandler;
            instance.m_InstanceId = instanceId;
            return instance;
        }

        /// <summary>
        /// 点
        /// </summary>
        private List<Unity.Mathematics.float2> _CenterPoints = new();

        [MemoryPackOrder(0)]
        public List<Unity.Mathematics.float2> CenterPoints
        {
            get => _CenterPoints;
            set {
                _CenterPoints = value;
                this.m_DirtyHandler?.Dirty(m_InstanceId, this);
            }
        }
        /// <summary>
        /// 边
        /// </summary>
        private List<Unity.Mathematics.float4> _Borders = new();

        [MemoryPackOrder(1)]
        public List<Unity.Mathematics.float4> Borders
        {
            get => _Borders;
            set {
                _Borders = value;
                this.m_DirtyHandler?.Dirty(m_InstanceId, this);
            }
        }
        /// <summary>
        /// 边宽度
        /// </summary>
        private int _AreaSize;

        [MemoryPackOrder(2)]
        public int AreaSize
        {
            get => _AreaSize;
            set {
                _AreaSize = value;
                this.m_DirtyHandler?.Dirty(m_InstanceId, this);
            }
        }
        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.m_DirtyHandler = null;
            this.m_InstanceId = default;
            
this._CenterPoints.Clear();
            this._Borders.Clear();
            this._AreaSize = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    /// <summary>
    /// 通讯协议
    /// </summary>
    // 1.获得玩家的视野世界信息
    [MemoryPackable]
    [Message(ClientMessage.C2B_PlayerGetAllAOIWorldData)]
    [ResponseType(nameof(B2C_PlayerGetAllAOIWorldData))]
    public partial class C2B_PlayerGetAllAOIWorldData : MessageObject, IClientRequest
    {
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

        public static C2B_PlayerGetAllAOIWorldData Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2B_PlayerGetAllAOIWorldData), isFromPool) as C2B_PlayerGetAllAOIWorldData;
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
    [Message(ClientMessage.B2C_PlayerGetAllAOIWorldData)]
    public partial class B2C_PlayerGetAllAOIWorldData : MessageObject, IClientResponse
    {
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

        public static B2C_PlayerGetAllAOIWorldData Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(B2C_PlayerGetAllAOIWorldData), isFromPool) as B2C_PlayerGetAllAOIWorldData;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public BattleWorld BattleWorld { get; set; }

        [MemoryPackOrder(4)]
        public List<BattleUnitEntity> BattleUnitEntity { get; set; } = new();

        [MemoryPackOrder(5)]
        public BattleUnitEntity MyPlayerUnitEntity { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.BattleWorld = default;
            this.BattleUnitEntity.Clear();
            this.MyPlayerUnitEntity = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    // 2.战斗场景玩家心跳
    [MemoryPackable]
    [Message(ClientMessage.C2B_PlayerBattleWorldPing)]
    [ResponseType(nameof(B2C_PlayerBattleWorldPing))]
    public partial class C2B_PlayerBattleWorldPing : MessageObject, IClientRequest
    {
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

        public static C2B_PlayerBattleWorldPing Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2B_PlayerBattleWorldPing), isFromPool) as C2B_PlayerBattleWorldPing;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public long ClientTime { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.ClientTime = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(ClientMessage.B2C_PlayerBattleWorldPing)]
    public partial class B2C_PlayerBattleWorldPing : MessageObject, IClientResponse
    {
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

        public static B2C_PlayerBattleWorldPing Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(B2C_PlayerBattleWorldPing), isFromPool) as B2C_PlayerBattleWorldPing;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public long SendClientTime { get; set; }

        /// <summary>
        /// 当前战斗世界的帧号
        /// </summary>
        [MemoryPackOrder(3)]
        public uint PlayerCurFrame { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.SendClientTime = default;
            this.PlayerCurFrame = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    // 3.玩家战斗脏数据推送
    [MemoryPackable]
    [Message(ClientMessage.L2C_PlayerAOIWorldDirtyPush)]
    public partial class L2C_PlayerAOIWorldDirtyPush : MessageObject, IMessage
    {
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

        public static L2C_PlayerAOIWorldDirtyPush Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(L2C_PlayerAOIWorldDirtyPush), isFromPool) as L2C_PlayerAOIWorldDirtyPush;
        }

        /// <summary>
        /// 开始帧数
        /// </summary>
        [MemoryPackOrder(0)]
        public uint startFrame { get; set; }

        /// <summary>
        /// 解锁帧数
        /// </summary>
        [MemoryPackOrder(1)]
        public uint endFrame { get; set; }

        /// <summary>
        /// 脏数据
        /// </summary>
        [MemoryPackOrder(2)]
        public List<BattleUnitEntity> DirtyUnitEntities { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.startFrame = default;
            this.endFrame = default;
            this.DirtyUnitEntities.Clear();

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
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

        public static Main2NetBattleLogin Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Main2NetBattleLogin), isFromPool) as Main2NetBattleLogin;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int OwnerFiberId { get; set; }

        /// <summary>
        /// 路由地址
        /// </summary>
        [MemoryPackOrder(2)]
        public string RouterAddress { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [MemoryPackOrder(2)]
        public string Address { get; set; }

        /// <summary>
        /// Token 令牌
        /// </summary>
        [MemoryPackOrder(3)]
        public string Token { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.OwnerFiberId = default;
            this.RouterAddress = default;
            this.Address = default;
            this.Token = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(ClientMessage.NetBattle2MainLogin)]
    public partial class NetBattle2MainLogin : MessageObject, IResponse
    {
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

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
    [Message(ClientMessage.C2B_Login)]
    [ResponseType(nameof(B2C_Login))]
    public partial class C2B_Login : MessageObject, ISessionRequest
    {
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

        public static C2B_Login Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2B_Login), isFromPool) as C2B_Login;
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
    [Message(ClientMessage.B2C_Login)]
    public partial class B2C_Login : MessageObject, ISessionResponse
    {
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

        public static B2C_Login Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(B2C_Login), isFromPool) as B2C_Login;
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
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

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
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

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

    // 1.玩家移动
    [MemoryPackable]
    [Message(ClientMessage.C2B_PlayerMoveOperationMessage)]
    public partial class C2B_PlayerMoveOperationMessage : MessageObject, IClientMessage
    {
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

        public static C2B_PlayerMoveOperationMessage Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2B_PlayerMoveOperationMessage), isFromPool) as C2B_PlayerMoveOperationMessage;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        /// <summary>
        /// 移动角度
        /// </summary>
        [MemoryPackOrder(1)]
        public int MoveAngle { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.MoveAngle = default;

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
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

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
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

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
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

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
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

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
    /// 客户端Main向Lobby网络线程发送消息
    /// </summary>
    [MemoryPackable]
    [Message(ClientMessage.Main2NetLobbyLogin)]
    [ResponseType(nameof(NetLobby2MainLogin))]
    public partial class Main2NetLobbyLogin : MessageObject, IRequest
    {
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

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
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

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
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

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
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

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
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

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
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

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
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

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
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

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
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

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
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

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
    [Message(ClientMessage.C2L_GetAllDataUnits)]
    [ResponseType(nameof(L2C_GetAllDataUnits))]
    public partial class C2L_GetAllDataUnits : MessageObject, IClientRequest
    {
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

        public static C2L_GetAllDataUnits Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2L_GetAllDataUnits), isFromPool) as C2L_GetAllDataUnits;
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
    [Message(ClientMessage.L2C_GetAllDataUnits)]
    public partial class L2C_GetAllDataUnits : MessageObject, IClientResponse
    {
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

        public static L2C_GetAllDataUnits Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(L2C_GetAllDataUnits), isFromPool) as L2C_GetAllDataUnits;
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
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

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
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

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
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

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
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

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
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

        public static L2C_MatchBattleSuccess Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(L2C_MatchBattleSuccess), isFromPool) as L2C_MatchBattleSuccess;
        }

        /// <summary>
        /// 路由地址
        /// </summary>
        [MemoryPackOrder(0)]
        public string RouterAddress { get; set; }

        /// <summary>
        /// 战斗服地址
        /// </summary>
        [MemoryPackOrder(1)]
        public string BattleAddress { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        [MemoryPackOrder(2)]
        public string Token { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RouterAddress = default;
            this.BattleAddress = default;
            this.Token = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    public static class ClientMessage
    {
        public const ushort BattleWorld = 10001;
        public const ushort BattleUnitEntity = 10002;
        public const ushort UnitEntityElemData = 10003;
        public const ushort UnitEntityCommonData = 10004;
        public const ushort UnitEntityPosition = 10005;
        public const ushort UnitEntityInfo = 10006;
        public const ushort UnitEntityPlayerInfo = 10007;
        public const ushort UnitEntityPlayerFrame = 10008;
        public const ushort UnitEntityPlayerOperationAction = 10009;
        public const ushort UnitEntityMapMessage = 10010;
        public const ushort GizmosDebugInfo = 10011;
        public const ushort C2B_PlayerGetAllAOIWorldData = 10012;
        public const ushort B2C_PlayerGetAllAOIWorldData = 10013;
        public const ushort C2B_PlayerBattleWorldPing = 10014;
        public const ushort B2C_PlayerBattleWorldPing = 10015;
        public const ushort L2C_PlayerAOIWorldDirtyPush = 10016;
        public const ushort Main2NetBattleLogin = 10017;
        public const ushort NetBattle2MainLogin = 10018;
        public const ushort C2B_Login = 10019;
        public const ushort B2C_Login = 10020;
        public const ushort C2B_PlayerReadyCompleted = 10021;
        public const ushort B2C_PlayerReadyCompleted = 10022;
        public const ushort C2B_PlayerMoveOperationMessage = 10023;
        public const ushort C2G_Ping = 10024;
        public const ushort G2C_Ping = 10025;
        public const ushort C2G_Benchmark = 10026;
        public const ushort G2C_Benchmark = 10027;
        public const ushort Main2NetLobbyLogin = 10028;
        public const ushort NetLobby2MainLogin = 10029;
        public const ushort C2A_Login = 10030;
        public const ushort A2C_Login = 10031;
        public const ushort C2L_LoginLobby = 10032;
        public const ushort L2C_LoginLobby = 10033;
        public const ushort G2C_SessionDisconnect = 10034;
        public const ushort HttpGetRouterResponse = 10035;
        public const ushort SyncDataUnitStruct = 10036;
        public const ushort DataUnitBytes = 10037;
        public const ushort C2L_GetAllDataUnits = 10038;
        public const ushort L2C_GetAllDataUnits = 10039;
        public const ushort L2C_SyncDirtyDataUnits = 10040;
        public const ushort RoleInfoUnitData = 10041;
        public const ushort C2L_StartMatchBattle = 10042;
        public const ushort L2C_StartMatchBattle = 10043;
        public const ushort L2C_MatchBattleSuccess = 10044;
    }
}