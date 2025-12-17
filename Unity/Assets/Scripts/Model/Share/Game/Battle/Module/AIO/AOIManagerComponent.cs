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
    }

    
    public class AIOCell
    {
        public long CellId { get; set; }
        
        // 所有UnitEntity
        public Dictionary<long, AOIUnitEntity> AllUnitEntities = new ();

        // 地块玩家
        public Dictionary<long, AOIUnitEntity> PlayerAOIEntities = new Dictionary<long, AOIUnitEntity>();
    }
    
    
    [ComponentOf(typeof(UnitEntity))]
    public class AOIUnitEntity : Entity, IAwake<float2, UETypeEnum>, IDestroy
    {
        public long CellId;
        
        public UETypeEnum UETypeEnum { get; set; }
    }

    [ComponentOf(typeof(UnitEntity))]
    public class PlayerAOISeeUnitEntity: Entity, IAwake
    {
        public List<long> EnterEntityIds = new();
        public List<long> LeaveEntityIds = new();
        public HashSet<long> ManageEntityIds = new();
    }
}