using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{    
    [ChildOf]
    public class ClientWorld : World
    {
        // AllEntity
        public Dictionary<long, UnitEntity> AllEntity = new ();
        
        // 玩家id-地图UnitEntity
        public Dictionary<long, UnitEntity> PlayerUnitEntities = new ();
        
        public Dictionary<long, UnitEntity> EvnUnitEntities = new ();
        
        public uint Frame;
        
        // 我的玩家信息
        public UnitEntity MyPlayer;
        
        // 地图 UnitEntity
        public UnitEntity UnitEntityMap;

        // Unit GameObject 资源
        public GameObject UnitGameObject;
    }    
}
