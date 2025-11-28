using System;
using System.Collections.Generic;
using Unity.Mathematics;

namespace ET
{
    [ComponentOf(typeof(LogicWorld))]
    public class AOIManagerComponent: Entity, IAwake<IDirtyHandler, ISyncHandler>, IDestroy
    {
        public LogicWorld LogicWorld { get; set; }
        
        public IDirtyHandler DirtyHandler { get; set; }
        
        public ISyncHandler SyncHandler { get; set; }
        
        public Dictionary<long, AIOCell> Cells { get; set; } = new();

        public Dictionary<long, AIOCell> DirtyCells = new();
    }

    
    public class AIOCell
    {
        public long CellId { get; set; }
        
        // 所有UnitEntity
        public Dictionary<long, AOIUnitEntity> AllUnitEntities = new ();
        
        // 玩家
        public Dictionary<long, AOIUnitEntity> PlayerUnitEntities = new();

        // 进入的Entity
        public HashSet<long> DirtyEnterEntities = new HashSet<long>();
        
        // 离开的Entity
        public HashSet<long> DirtyLeaveEntities = new HashSet<long>();
    }
    
    
    [ComponentOf(typeof(UnitEntity))]
    public class AOIUnitEntity : Entity, IAwake<float2>, IDestroy
    {
        public long CellId;
    }
}