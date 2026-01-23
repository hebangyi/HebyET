// This Is Auto Generate, Do Not Edit!
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
    // 常规信息
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

        private UETypeEnum _UnitEntityType;

        [MemoryPackOrder(0)]
        public UETypeEnum UnitEntityType
        {
            get => _UnitEntityType;
            set {
                _UnitEntityType = value;
                this.Dirty();
            }
        }
        private UELayerTypeEnum _UELayerTypeEnum;

        [MemoryPackOrder(1)]
        public UELayerTypeEnum UELayerTypeEnum
        {
            get => _UELayerTypeEnum;
            set {
                _UELayerTypeEnum = value;
                this.Dirty();
            }
        }
        private Dictionary<string, string> _Datas = new();

        [MongoDB.Bson.Serialization.Attributes.BsonDictionaryOptions(MongoDB.Bson.Serialization.Options.DictionaryRepresentation.ArrayOfArrays)]
        [MemoryPackOrder(2)]
        public Dictionary<string, string> Datas 
        {
            get => _Datas;
            set {
                _Datas = value;
                this.Dirty();
            }
        }
        /// <summary>
        /// 配置ID
        /// </summary>
        private long _ConfigId;

        [MemoryPackOrder(3)]
        public long ConfigId
        {
            get => _ConfigId;
            set {
                _ConfigId = value;
                this.Dirty();
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
            this._UELayerTypeEnum = default;
            this._Datas.Clear();
            this._ConfigId = default;

            ObjectPool.Instance.Recycle(this);
        }

        public void Dirty()
        {
            this.m_DirtyHandler?.Dirty(m_InstanceId, this);
        }

    }

    // 动画状态
    [MemoryPackable]
    [Message(ClientMessage.UnitEntityAnimationStateData)]
    public partial class UnitEntityAnimationStateData : MessageObject, IUnitEntityElemData
    {
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

        public static UnitEntityAnimationStateData Create(long instanceId, IDirtyHandler dirtyHandler, bool isFromPool = false)
        {
            var instance = ObjectPool.Instance.Fetch(typeof(UnitEntityAnimationStateData), isFromPool) as UnitEntityAnimationStateData;
            instance.m_DirtyHandler = dirtyHandler;
            instance.m_InstanceId = instanceId;
            return instance;
        }

        /// <summary>
        /// 动画状态
        /// </summary>
        private AnimateStateEnum _AnimateState;

        [MemoryPackOrder(0)]
        public AnimateStateEnum AnimateState
        {
            get => _AnimateState;
            set {
                _AnimateState = value;
                this.Dirty();
            }
        }
        /// <summary>
        /// 当前状态开始的逻辑帧
        /// </summary>
        private uint _CurrentStateStartFrame;

        [MemoryPackOrder(1)]
        public uint CurrentStateStartFrame
        {
            get => _CurrentStateStartFrame;
            set {
                _CurrentStateStartFrame = value;
                this.Dirty();
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

            this._AnimateState = default;
            this._CurrentStateStartFrame = default;

            ObjectPool.Instance.Recycle(this);
        }

        public void Dirty()
        {
            this.m_DirtyHandler?.Dirty(m_InstanceId, this);
        }

    }

    // 位置
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

        /// <summary>
        /// 位置
        /// </summary>
        private Unity.Mathematics.float2 _Position;

        [MemoryPackOrder(0)]
        public Unity.Mathematics.float2 Position
        {
            get => _Position;
            set {
                _Position = value;
                this.Dirty();
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

            ObjectPool.Instance.Recycle(this);
        }

        public void Dirty()
        {
            this.m_DirtyHandler?.Dirty(m_InstanceId, this);
        }

    }

    // 朝向角度
    [MemoryPackable]
    [Message(ClientMessage.UnitEntityTowardAngle)]
    public partial class UnitEntityTowardAngle : MessageObject, IUnitEntityElemData
    {
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

        public static UnitEntityTowardAngle Create(long instanceId, IDirtyHandler dirtyHandler, bool isFromPool = false)
        {
            var instance = ObjectPool.Instance.Fetch(typeof(UnitEntityTowardAngle), isFromPool) as UnitEntityTowardAngle;
            instance.m_DirtyHandler = dirtyHandler;
            instance.m_InstanceId = instanceId;
            return instance;
        }

        /// <summary>
        /// 朝向 -180-180
        /// </summary>
        private short _TowardAngle;

        [MemoryPackOrder(0)]
        public short TowardAngle
        {
            get => _TowardAngle;
            set {
                _TowardAngle = value;
                this.Dirty();
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

            this._TowardAngle = default;

            ObjectPool.Instance.Recycle(this);
        }

        public void Dirty()
        {
            this.m_DirtyHandler?.Dirty(m_InstanceId, this);
        }

    }

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
                this.Dirty();
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
                this.Dirty();
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
                this.Dirty();
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

        public void Dirty()
        {
            this.m_DirtyHandler?.Dirty(m_InstanceId, this);
        }

    }

    // 玩家数据
    [MemoryPackable]
    [Message(ClientMessage.UnitEntityCameraData)]
    public partial class UnitEntityCameraData : MessageObject, IUnitEntityElemData
    {
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

        public static UnitEntityCameraData Create(long instanceId, IDirtyHandler dirtyHandler, bool isFromPool = false)
        {
            var instance = ObjectPool.Instance.Fetch(typeof(UnitEntityCameraData), isFromPool) as UnitEntityCameraData;
            instance.m_DirtyHandler = dirtyHandler;
            instance.m_InstanceId = instanceId;
            return instance;
        }

        /// <summary>
        /// 相机偏移角度
        /// </summary>
        private short _CameraAngleOffSet;

        [MemoryPackOrder(0)]
        public short CameraAngleOffSet
        {
            get => _CameraAngleOffSet;
            set {
                _CameraAngleOffSet = value;
                this.Dirty();
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

            this._CameraAngleOffSet = default;

            ObjectPool.Instance.Recycle(this);
        }

        public void Dirty()
        {
            this.m_DirtyHandler?.Dirty(m_InstanceId, this);
        }

    }

    // 玩家技能
    [MemoryPackable]
    [Message(ClientMessage.UnitEntityPlayerSkill)]
    public partial class UnitEntityPlayerSkill : MessageObject, IUnitEntityElemData
    {
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

        public static UnitEntityPlayerSkill Create(long instanceId, IDirtyHandler dirtyHandler, bool isFromPool = false)
        {
            var instance = ObjectPool.Instance.Fetch(typeof(UnitEntityPlayerSkill), isFromPool) as UnitEntityPlayerSkill;
            instance.m_DirtyHandler = dirtyHandler;
            instance.m_InstanceId = instanceId;
            return instance;
        }

        /// <summary>
        /// 技能数据
        /// </summary>
        private List<UnitEntityPlayerSkillDataItem> _SkillDataItems = new();

        [MemoryPackOrder(0)]
        public List<UnitEntityPlayerSkillDataItem> SkillDataItems
        {
            get => _SkillDataItems;
            set {
                _SkillDataItems = value;
                this.Dirty();
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

            this._SkillDataItems.Clear();

            ObjectPool.Instance.Recycle(this);
        }

        public void Dirty()
        {
            this.m_DirtyHandler?.Dirty(m_InstanceId, this);
        }

    }

    // 玩家技能DataItem
    [MemoryPackable]
    [Message(ClientMessage.UnitEntityPlayerSkillDataItem)]
    public partial class UnitEntityPlayerSkillDataItem : MessageObject
    {
        private long m_InstanceId;

        public static UnitEntityPlayerSkillDataItem Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(UnitEntityPlayerSkillDataItem), isFromPool) as UnitEntityPlayerSkillDataItem;
        }

        [MemoryPackOrder(0)]
        public long SkillId { get; set; }

        /// <summary>
        /// 技能状态
        /// </summary>
        [MemoryPackOrder(1)]
        public SkillStatusEnum SkillStatusEnum { get; set; }

        /// <summary>
        /// 如果开始 技能当前的帧数是多少
        /// </summary>
        [MemoryPackOrder(2)]
        public uint ActiveFrame { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }
            this.SkillId = default;
            this.SkillStatusEnum = default;
            this.ActiveFrame = default;

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
        /// 边宽度
        /// </summary>
        private int _AreaSize;

        [MemoryPackOrder(0)]
        public int AreaSize
        {
            get => _AreaSize;
            set {
                _AreaSize = value;
                this.Dirty();
            }
        }
        /// <summary>
        /// 地块信息
        /// </summary>
        private PlantInfo _PlantInfo;

        [MemoryPackOrder(1)]
        public PlantInfo PlantInfo
        {
            get => _PlantInfo;
            set {
                _PlantInfo = value;
                this.Dirty();
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

            this._AreaSize = default;
            this._PlantInfo = default;

            ObjectPool.Instance.Recycle(this);
        }

        public void Dirty()
        {
            this.m_DirtyHandler?.Dirty(m_InstanceId, this);
        }

    }

    [MemoryPackable]
    [Message(ClientMessage.PlantInfo)]
    public partial class PlantInfo : MessageObject
    {
        private long m_InstanceId;

        public static PlantInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(PlantInfo), isFromPool) as PlantInfo;
        }

        /// <summary>
        /// 网格
        /// </summary>
        [MemoryPackOrder(1)]
        public List<CellInfo> CellInfos { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }
            this.CellInfos.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(ClientMessage.CellInfo)]
    public partial class CellInfo : MessageObject
    {
        private long m_InstanceId;

        public static CellInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(CellInfo), isFromPool) as CellInfo;
        }

        [MemoryPackOrder(0)]
        public Unity.Mathematics.float2 CenterPoint { get; set; }

        /// <summary>
        /// 边
        /// </summary>
        [MemoryPackOrder(1)]
        public List<Unity.Mathematics.float4> Borders { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }
            this.CenterPoint = default;
            this.Borders.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    // 地块辅助线信息
    [MemoryPackable]
    [Message(ClientMessage.GizmosPlantInfo)]
    public partial class GizmosPlantInfo : MessageObject, IUnitEntityElemData
    {
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

        public static GizmosPlantInfo Create(long instanceId, IDirtyHandler dirtyHandler, bool isFromPool = false)
        {
            var instance = ObjectPool.Instance.Fetch(typeof(GizmosPlantInfo), isFromPool) as GizmosPlantInfo;
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
                this.Dirty();
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
                this.Dirty();
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
                this.Dirty();
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

        public void Dirty()
        {
            this.m_DirtyHandler?.Dirty(m_InstanceId, this);
        }

    }

    // 玩家AOI辅助线
    [MemoryPackable]
    [Message(ClientMessage.GizmosPlayerAOICell)]
    public partial class GizmosPlayerAOICell : MessageObject, IUnitEntityElemData
    {
        private IDirtyHandler m_DirtyHandler;
        private long m_InstanceId;

        public static GizmosPlayerAOICell Create(long instanceId, IDirtyHandler dirtyHandler, bool isFromPool = false)
        {
            var instance = ObjectPool.Instance.Fetch(typeof(GizmosPlayerAOICell), isFromPool) as GizmosPlayerAOICell;
            instance.m_DirtyHandler = dirtyHandler;
            instance.m_InstanceId = instanceId;
            return instance;
        }

        /// <summary>
        /// Cell Ids
        /// </summary>
        private List<long> _CellIds = new();

        [MemoryPackOrder(0)]
        public List<long> CellIds
        {
            get => _CellIds;
            set {
                _CellIds = value;
                this.Dirty();
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

            this._CellIds.Clear();

            ObjectPool.Instance.Recycle(this);
        }

        public void Dirty()
        {
            this.m_DirtyHandler?.Dirty(m_InstanceId, this);
        }

    }

    // 未登录的常规协议
    /// <summary>
    /// 网络
    /// </summary>
    [MemoryPackable]
    [Message(ClientMessage.C2G_Ping)]
    [ResponseType(nameof(G2C_Ping), "Battle", "BattleRole")]
    public partial class C2G_Ping : MessageObject, ISessionRequest
    {
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
    /// 客户端Main向网络线程发送消息
    /// </summary>
    [MemoryPackable]
    [Message(ClientMessage.Main2NetBattleLogin)]
    [ResponseType(nameof(NetBattle2MainLogin))]
    public partial class Main2NetBattleLogin : MessageObject, IRequest
    {
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

    /// <summary>
    /// 通讯协议
    /// </summary>
    // 1.获得玩家的视野世界信息
    [MemoryPackable]
    [Message(ClientMessage.C2B_PlayerGetAllAOIWorldData)]
    [ResponseType(nameof(B2C_PlayerGetAllAOIWorldData))]
    public partial class C2B_PlayerGetAllAOIWorldData : MessageObject, IClientRequest
    {
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
        public List<BattleUnitEntity> AOIBattleUnitEntity { get; set; } = new();

        /// <summary>
        /// 环境相关的UnitEntity 变化
        /// </summary>
        [MemoryPackOrder(5)]
        public List<BattleUnitEntity> BattleFieldUnitEntity { get; set; } = new();

        /// <summary>
        /// 我的UnitEntity
        /// </summary>
        [MemoryPackOrder(6)]
        public BattleUnitEntity MyPlayerUnitEntity { get; set; }

        /// <summary>
        /// 逻辑间隔
        /// </summary>
        [MemoryPackOrder(7)]
        public int LogicInterval { get; set; }

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
            this.AOIBattleUnitEntity.Clear();
            this.BattleFieldUnitEntity.Clear();
            this.MyPlayerUnitEntity = default;
            this.LogicInterval = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    // 2.战斗场景玩家心跳
    [MemoryPackable]
    [Message(ClientMessage.C2B_PlayerBattleWorldPing)]
    [ResponseType(nameof(B2C_PlayerBattleWorldPing))]
    public partial class C2B_PlayerBattleWorldPing : MessageObject, IClientRequest
    {
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
        private long m_InstanceId;

        public static L2C_PlayerAOIWorldDirtyPush Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(L2C_PlayerAOIWorldDirtyPush), isFromPool) as L2C_PlayerAOIWorldDirtyPush;
        }

        /// <summary>
        /// 开始帧数
        /// </summary>
        [MemoryPackOrder(0)]
        public uint LastSyncFrame { get; set; }

        /// <summary>
        /// 解锁帧数
        /// </summary>
        [MemoryPackOrder(1)]
        public uint CurrentSyncFrame { get; set; }

        /// <summary>
        /// 添加数据
        /// </summary>
        [MemoryPackOrder(2)]
        public List<BattleUnitEntity> AddUnitEntiities { get; set; } = new();

        /// <summary>
        /// 脏数据
        /// </summary>
        [MemoryPackOrder(3)]
        public List<BattleUnitEntity> DirtyUnitEntities { get; set; } = new();

        /// <summary>
        /// 死亡数据
        /// </summary>
        [MemoryPackOrder(4)]
        public List<long> DeleteUnitEntites { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }
            this.LastSyncFrame = default;
            this.CurrentSyncFrame = default;
            this.AddUnitEntiities.Clear();
            this.DirtyUnitEntities.Clear();
            this.DeleteUnitEntites.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    // 4.玩家操作脏数据
    [MemoryPackable]
    [Message(ClientMessage.C2B_PlayerUploadDirtyElemData)]
    [ResponseType(nameof(B2C_PlayerUploadDirtyElemData))]
    public partial class C2B_PlayerUploadDirtyElemData : MessageObject, IClientRequest
    {
        private long m_InstanceId;

        public static C2B_PlayerUploadDirtyElemData Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2B_PlayerUploadDirtyElemData), isFromPool) as C2B_PlayerUploadDirtyElemData;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        /// <summary>
        /// 玩家用户UnitEntity 脏数据
        /// </summary>
        [MemoryPackOrder(1)]
        public BattleUnitEntity BattleUnitEntity { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }
            this.RpcId = default;
            this.BattleUnitEntity = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(ClientMessage.B2C_PlayerUploadDirtyElemData)]
    public partial class B2C_PlayerUploadDirtyElemData : MessageObject, IClientResponse
    {
        private long m_InstanceId;

        public static B2C_PlayerUploadDirtyElemData Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(B2C_PlayerUploadDirtyElemData), isFromPool) as B2C_PlayerUploadDirtyElemData;
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

    // 5.玩家准备好进入战斗
    [MemoryPackable]
    [Message(ClientMessage.C2B_PlayeBattleReday)]
    public partial class C2B_PlayeBattleReday : MessageObject, IClientRequest
    {
        private long m_InstanceId;

        public static C2B_PlayeBattleReday Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2B_PlayeBattleReday), isFromPool) as C2B_PlayeBattleReday;
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
    [Message(ClientMessage.B2C_PlayeBattleReday)]
    public partial class B2C_PlayeBattleReday : MessageObject, IClientResponse
    {
        private long m_InstanceId;

        public static B2C_PlayeBattleReday Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(B2C_PlayeBattleReday), isFromPool) as B2C_PlayeBattleReday;
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

    // 1.玩家使用技能
    [MemoryPackable]
    [Message(ClientMessage.C2B_PlayerUseSkill)]
    [ResponseType(nameof(B2C_PlayerUseSkill), "Battle", "BattleRole")]
    public partial class C2B_PlayerUseSkill : MessageObject, IClientRequest
    {
        private long m_InstanceId;

        public static C2B_PlayerUseSkill Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2B_PlayerUseSkill), isFromPool) as C2B_PlayerUseSkill;
        }

        [MemoryPackOrder(98)]
        public int RpcId { get; set; }

        [MemoryPackOrder(0)]
        public long SkillId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }
            this.RpcId = default;
            this.SkillId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(ClientMessage.B2C_PlayerUseSkill)]
    public partial class B2C_PlayerUseSkill : MessageObject, IClientResponse
    {
        private long m_InstanceId;

        public static B2C_PlayerUseSkill Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(B2C_PlayerUseSkill), isFromPool) as B2C_PlayerUseSkill;
        }

        [MemoryPackOrder(96)]
        public int RpcId { get; set; }

        [MemoryPackOrder(97)]
        public int Error { get; set; }

        [MemoryPackOrder(98)]
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

    // 2.玩家移动
    [MemoryPackable]
    [Message(ClientMessage.C2B_PlayerMove)]
    public partial class C2B_PlayerMove : MessageObject, IClientRequest
    {
        private long m_InstanceId;

        public static C2B_PlayerMove Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2B_PlayerMove), isFromPool) as C2B_PlayerMove;
        }

        [MemoryPackOrder(98)]
        public int RpcId { get; set; }

        /// <summary>
        /// 目标点
        /// </summary>
        [MemoryPackOrder(0)]
        public Unity.Mathematics.float2 Position { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }
            this.RpcId = default;
            this.Position = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(ClientMessage.B2C_PlayerMove)]
    public partial class B2C_PlayerMove : MessageObject, IClientResponse
    {
        private long m_InstanceId;

        public static B2C_PlayerMove Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(B2C_PlayerMove), isFromPool) as B2C_PlayerMove;
        }

        [MemoryPackOrder(96)]
        public int RpcId { get; set; }

        [MemoryPackOrder(97)]
        public int Error { get; set; }

        [MemoryPackOrder(98)]
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

    // 3.玩家停止移动
    [MemoryPackable]
    [Message(ClientMessage.C2B_PlayerMoveStop)]
    [ResponseType(nameof(B2C_PlayerMoveStop), "Battle", "BattleRole")]
    public partial class C2B_PlayerMoveStop : MessageObject, IClientRequest
    {
        private long m_InstanceId;

        public static C2B_PlayerMoveStop Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2B_PlayerMoveStop), isFromPool) as C2B_PlayerMoveStop;
        }

        [MemoryPackOrder(98)]
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
    [Message(ClientMessage.B2C_PlayerMoveStop)]
    public partial class B2C_PlayerMoveStop : MessageObject, IClientResponse
    {
        private long m_InstanceId;

        public static B2C_PlayerMoveStop Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(B2C_PlayerMoveStop), isFromPool) as B2C_PlayerMoveStop;
        }

        [MemoryPackOrder(96)]
        public int RpcId { get; set; }

        [MemoryPackOrder(97)]
        public int Error { get; set; }

        [MemoryPackOrder(98)]
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

    // 1.开始世界进行
    [MemoryPackable]
    [Message(ClientMessage.C2B_DebugStartWorld)]
    public partial class C2B_DebugStartWorld : MessageObject, IClientRequest
    {
        private long m_InstanceId;

        public static C2B_DebugStartWorld Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2B_DebugStartWorld), isFromPool) as C2B_DebugStartWorld;
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
    [Message(ClientMessage.B2C_DebugStartWorld)]
    public partial class B2C_DebugStartWorld : MessageObject, IClientResponse
    {
        private long m_InstanceId;

        public static B2C_DebugStartWorld Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(B2C_DebugStartWorld), isFromPool) as B2C_DebugStartWorld;
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

    // 2.世界暂停
    [MemoryPackable]
    [Message(ClientMessage.C2B_DebugWorldPlush)]
    public partial class C2B_DebugWorldPlush : MessageObject, IClientRequest
    {
        private long m_InstanceId;

        public static C2B_DebugWorldPlush Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2B_DebugWorldPlush), isFromPool) as C2B_DebugWorldPlush;
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
    [Message(ClientMessage.B2C_DebugWorldPlush)]
    public partial class B2C_DebugWorldPlush : MessageObject, IClientResponse
    {
        private long m_InstanceId;

        public static B2C_DebugWorldPlush Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(B2C_DebugWorldPlush), isFromPool) as B2C_DebugWorldPlush;
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

    public static class ClientMessage
    {
        public const ushort BattleWorld = 10001;
        public const ushort BattleUnitEntity = 10002;
        public const ushort UnitEntityElemData = 10003;
        public const ushort UnitEntityCommonData = 10004;
        public const ushort UnitEntityAnimationStateData = 10005;
        public const ushort UnitEntityPosition = 10006;
        public const ushort UnitEntityTowardAngle = 10007;
        public const ushort UnitEntityPlayerInfo = 10008;
        public const ushort UnitEntityCameraData = 10009;
        public const ushort UnitEntityPlayerSkill = 10010;
        public const ushort UnitEntityPlayerSkillDataItem = 10011;
        public const ushort UnitEntityMapMessage = 10012;
        public const ushort PlantInfo = 10013;
        public const ushort CellInfo = 10014;
        public const ushort GizmosPlantInfo = 10015;
        public const ushort GizmosPlayerAOICell = 10016;
        public const ushort C2G_Ping = 10017;
        public const ushort G2C_Ping = 10018;
        public const ushort C2G_Benchmark = 10019;
        public const ushort G2C_Benchmark = 10020;
        public const ushort Main2NetBattleLogin = 10021;
        public const ushort NetBattle2MainLogin = 10022;
        public const ushort C2B_Login = 10023;
        public const ushort B2C_Login = 10024;
        public const ushort C2B_PlayerReadyCompleted = 10025;
        public const ushort B2C_PlayerReadyCompleted = 10026;
        public const ushort C2B_PlayerGetAllAOIWorldData = 10027;
        public const ushort B2C_PlayerGetAllAOIWorldData = 10028;
        public const ushort C2B_PlayerBattleWorldPing = 10029;
        public const ushort B2C_PlayerBattleWorldPing = 10030;
        public const ushort L2C_PlayerAOIWorldDirtyPush = 10031;
        public const ushort C2B_PlayerUploadDirtyElemData = 10032;
        public const ushort B2C_PlayerUploadDirtyElemData = 10033;
        public const ushort C2B_PlayeBattleReday = 10034;
        public const ushort B2C_PlayeBattleReday = 10035;
        public const ushort C2B_PlayerUseSkill = 10036;
        public const ushort B2C_PlayerUseSkill = 10037;
        public const ushort C2B_PlayerMove = 10038;
        public const ushort B2C_PlayerMove = 10039;
        public const ushort C2B_PlayerMoveStop = 10040;
        public const ushort B2C_PlayerMoveStop = 10041;
        public const ushort Main2NetLobbyLogin = 10042;
        public const ushort NetLobby2MainLogin = 10043;
        public const ushort C2A_Login = 10044;
        public const ushort A2C_Login = 10045;
        public const ushort C2L_LoginLobby = 10046;
        public const ushort L2C_LoginLobby = 10047;
        public const ushort G2C_SessionDisconnect = 10048;
        public const ushort HttpGetRouterResponse = 10049;
        public const ushort SyncDataUnitStruct = 10050;
        public const ushort DataUnitBytes = 10051;
        public const ushort C2L_GetAllDataUnits = 10052;
        public const ushort L2C_GetAllDataUnits = 10053;
        public const ushort L2C_SyncDirtyDataUnits = 10054;
        public const ushort RoleInfoUnitData = 10055;
        public const ushort C2L_StartMatchBattle = 10056;
        public const ushort L2C_StartMatchBattle = 10057;
        public const ushort L2C_MatchBattleSuccess = 10058;
        public const ushort C2B_DebugStartWorld = 10059;
        public const ushort B2C_DebugStartWorld = 10060;
        public const ushort C2B_DebugWorldPlush = 10061;
        public const ushort B2C_DebugWorldPlush = 10062;
    }
}