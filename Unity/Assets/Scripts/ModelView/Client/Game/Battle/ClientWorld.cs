using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{    
    [ChildOf]
    public class ClientWorld : World
    {
        // AllEntity
        public Dictionary<long, UnitEntity> AllEntities = new ();
        // 玩家id-地图UnitEntity
        public Dictionary<long, UnitEntity> PlayerUnitEntities = new ();
        
        public Dictionary<long, UnitEntity> EvnUnitEntities = new ();
        
        public uint Frame;
        
        // 我的玩家信息
        public UnitEntity MainPlayer;
        
        public long MainPlayerId;
        
        // 地图 PlantMessage
        public UnitEntity UnitEntityMap;

        // Unit GameObject 资源
        public GameObject UnitGameObject;

        // Unit Monster GameObject 资源
        public GameObject UnitMonsterGameObject;

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
