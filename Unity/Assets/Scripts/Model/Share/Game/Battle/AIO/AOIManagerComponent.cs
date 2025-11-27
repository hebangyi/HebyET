using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(LogicWorld))]
    public class AOIManagerComponent: Entity, IAwake, IDestroy
    {
        public LogicWorld LogicWorld { get; set; }
        public Dictionary<long, AIOCell> Cells { get; set; } = new();
    }


    public class AIOCell
    {
        public long CellId { get; set; }
        
        // 所有UnitEntity
        public Dictionary<long, AOIUnitEntity> AllUnitEntities = new ();
        
        // 玩家
        public Dictionary<long, AOIUnitEntity> PlayerUnitEntities = new();
    }
    
    
    [ComponentOf(typeof(UnitEntity))]
    public class AOIUnitEntity : Entity, IAwake<long>, IDestroy
    {
        public long CellId;
    }
}