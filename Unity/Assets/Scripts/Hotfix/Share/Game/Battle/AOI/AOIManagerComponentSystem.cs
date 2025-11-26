using System.Collections.Generic;

namespace ET
{
    [FriendOf(typeof(AOIManagerComponent))]
    [EntitySystemOf(typeof(AOIManagerComponent))]
    public static partial class AOIManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this AOIManagerComponent self)
        {
            self.LogicWorld = self.GetParent<LogicWorld>();
        }
        
        [EntitySystem]
        private static void Destroy(this AOIManagerComponent self)
        {
            foreach (var cell in self.Cells.Values)
            {
                cell.CellId = 0;
                cell.AllUnitEntities.Clear();
                
                ObjectPool.Instance.Recycle(cell);
            }
        }
        
        public static AIOCell GetOrCreateCellData(this AOIManagerComponent aoiManagerComponent, long cellId)
        {
            var cellData = aoiManagerComponent.GetCellData(cellId);
            if (cellData != null)
            {
                return cellData;
            }

            AIOCell aioCellData = ObjectPool.Instance.Fetch<AIOCell>();
            cellData.CellId = cellId;
            aoiManagerComponent.Cells[cellId] = aioCellData;
            return aioCellData;
        }

        public static AIOCell GetCellData(this AOIManagerComponent aoiManagerComponent, long cellId)
        {
            return aoiManagerComponent.Cells.GetValueOrDefault(cellId);
        }
        
        
        
        
        
        public static void BindUnitEntity(this AOIManagerComponent aoiManagerComponent, AOIUnitEntity aoiUnitEntity)
        {
            var aoiCell = aoiManagerComponent.GetOrCreateCellData(aoiUnitEntity.CellId);
            aoiCell.AllUnitEntities[aoiUnitEntity.CellId] = aoiUnitEntity;

            var unitEntity = aoiUnitEntity.GetParent<UnitEntity>();
            var unitEntityCommonData = unitEntity.GetUnitEntityElemData<UnitEntityCommonData>();

            if (unitEntityCommonData.UnitEntityType == UETypeEnum.Player)
            {
                aoiCell.PlayerUnitEntities[aoiUnitEntity.CellId] = aoiUnitEntity;
            }
        }
        

        public static void UnBindUnitEntity(this AOIManagerComponent aoiManagerComponent, AOIUnitEntity aoiUnitEntity)
        {
            var aoiCell = aoiManagerComponent.GetOrCreateCellData(aoiUnitEntity.CellId);
            aoiCell.AllUnitEntities.Remove(aoiUnitEntity.CellId);
            aoiCell.PlayerUnitEntities.Remove(aoiUnitEntity.CellId);
        }
    }
}