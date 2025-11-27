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

            self.Cells.Clear();
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
            var aoiCell = aoiManagerComponent.GetCellData(aoiUnitEntity.CellId);
            if (aoiCell == null)
            {
                return;
            }

            aoiCell.AllUnitEntities.Remove(aoiUnitEntity.CellId);
            aoiCell.PlayerUnitEntities.Remove(aoiUnitEntity.CellId);
        }

        public static void AwakeCellUnitEntity(this AOIManagerComponent aoiManagerComponent, AOIUnitEntity aoiUnitEntity, long toCellId)
        {
            aoiManagerComponent.MoveCell(aoiUnitEntity, toCellId);
        }



        public static void MoveCell(this AOIManagerComponent aoiManagerComponent, AOIUnitEntity aoiUnitEntity, long newCellId)
        {
            if (aoiUnitEntity.CellId == newCellId)
            {
                return;
            }

            var oldCellId = aoiUnitEntity.CellId;
            var (oldX, oldY) = AOIHelper.GetCellXY(oldCellId);
            var (newX, newY) = AOIHelper.GetCellXY(newCellId);

            aoiManagerComponent.UnBindUnitEntity(aoiUnitEntity);
            for (long x = oldX - 1; x <= oldX + 1; x++)
            {
                for (long y = oldY - 1; y <= oldY + 1; y++)
                {
                    if (x < newX - 1 && x > newX + 1 && y < newY - 1 && y > newY + 1)
                    {
                        long cellId = AOIHelper.GetCellId((int)x, (int)y);
                        var leaveCellData = aoiManagerComponent.GetCellData(cellId);
                        if (leaveCellData != null)
                        {
                            aoiManagerComponent.LeaveCellScope(leaveCellData, aoiUnitEntity);
                        }
                    }
                }
            }

            for (long x = newX - 1; x <= newX + 1; x++)
            {
                for (long y = newY - 1; y <= newY + 1; y++)
                {
                    if (x < oldX - 1 && x > oldX + 1 && y < oldY - 1 && y > oldY + 1)
                    {
                        long cellId = AOIHelper.GetCellId((int)x, (int)y);
                        var enterCellData = aoiManagerComponent.GetOrCreateCellData(cellId);
                        aoiManagerComponent.EnterCellScope(enterCellData, aoiUnitEntity);
                    }
                }
            }
            
            
            aoiUnitEntity.CellId = newCellId;
            aoiManagerComponent.BindUnitEntity(aoiUnitEntity);
        }

        public static void DestroyCellUnitEntity(this AOIManagerComponent aoiManagerComponent, AOIUnitEntity aoiUnitEntity)
        {
            long centerCellId = aoiUnitEntity.CellId;
            aoiManagerComponent.UnBindUnitEntity(aoiUnitEntity);
            
            var (oldX, oldY) = AOIHelper.GetCellXY(centerCellId);
            for (long x = oldX - 1; x <= oldX + 1; x++)
            {
                for (long y = oldY - 1; y <= oldY + 1; y++)
                {
                    long cellId = AOIHelper.GetCellId((int)x, (int)y);
                    var leaveCellData = aoiManagerComponent.GetCellData(cellId);
                    if (leaveCellData != null)
                    {
                        aoiManagerComponent.LeaveCellScope(leaveCellData, aoiUnitEntity);
                    }
                }
            }
            
            aoiUnitEntity.CellId = 0;
        }
        
        public static List<AOIUnitEntity> GetAllWatchUnitEntities(this AOIManagerComponent aoiManagerComponent, AOIUnitEntity aoiUnitEntity)
        {
            var centerCellId = aoiUnitEntity.CellId;
            var (newX, newY) = AOIHelper.GetCellXY(centerCellId);
            
            List<AOIUnitEntity> watchUnitEntities = new List<AOIUnitEntity>();
            for (long x = newX - 1; x <= newX + 1; x++)
            {
                for (long y = newY - 1; y <= newY + 1; y++)
                {
                    long cellId = AOIHelper.GetCellId((int)x, (int)y);
                    var cellData = aoiManagerComponent.Cells.GetValueOrDefault(cellId);
                    if (cellData != null)
                    {
                        watchUnitEntities.AddRange(cellData.AllUnitEntities.Values);
                    }
                }
            }

            return watchUnitEntities;
        }
        
        

        public static void EnterCellScope(this AOIManagerComponent aoiManagerComponent, AIOCell enterCell, AOIUnitEntity aoiUnitEntity)
        {
            foreach (var playerUnitEntity in enterCell.PlayerUnitEntities.Values)
            {
                AOIUnitEntity playerUnit = playerUnitEntity;
                // TODO 通知 玩家进入 
            }
        }

        public static void LeaveCellScope(this AOIManagerComponent aoiManagerComponent, AIOCell cell, AOIUnitEntity aoiUnitEntity)
        {
            foreach (var playerUnitEntity in cell.PlayerUnitEntities.Values)
            {
                AOIUnitEntity playerUnit = playerUnitEntity;
                // TODO 通知 玩家Player 离开 
            }

            // TODO 如果没有管理的UnitEntity 可以考虑清除
        }
    }
}