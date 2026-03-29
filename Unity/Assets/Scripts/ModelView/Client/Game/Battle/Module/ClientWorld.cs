using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{    
    [ChildOf]
    public class ClientWorld : World
    {
        // AllEntity
        public Dictionary<long, ClientUnitEntity> AllEntities = new ();
        // 玩家id-地图UnitEntity
        public Dictionary<long, ClientUnitEntity> PlayerUnitEntities = new ();
        
        public Dictionary<long, ClientUnitEntity> EvnUnitEntities = new ();
        
        // 逻辑帧
        public uint Frame;

        // 逻辑帧间隔
        public int LogicInterval;
        
        // 我的玩家信息
        public ClientUnitEntity MainPlayer;
        
        public long MainPlayerId;
        
        // 地图 PlantMessage
        public ClientUnitEntity UnitEntityMap;

        // Unit GameObject 资源
        public GameObject UnitGameObject;

        // Unit Monster GameObject 资源
        public GameObject UnitMonsterGameObject;

        // Unit Player GameObejct 资源
        public GameObject UnitPlayerGameObject;

        // 缓存的 DirtyMessage
        public List<L2C_PlayerAOIWorldDirtyPush> CacheDirtyMessage = new ();

        // Client World 状态
        public ClientWorldStatusEnum ClientWorldStatusEnum;
    }

    public enum ClientWorldStatusEnum
    {
        None = 0,
        InitData,
        Run,
    }

    public enum UnitEntityAnimationToward
    {
        // Up,
        // Down,
        Left,
        Right
    }
}
