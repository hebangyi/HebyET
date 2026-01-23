using System.Collections.Generic;

namespace ET
{
    [FriendOf(typeof(AOIManagerComponent))]
    [EntitySystemOf(typeof(AOIManagerComponent))]
    public static partial class AOIManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this AOIManagerComponent self, IDirtyHandler dirtyHandler, ISyncHandler syncHandler)
        {
            self.LogicWorld = self.GetParent<LogicWorld>();
            self.DirtyHandler = dirtyHandler;
            self.SyncHandler = syncHandler;
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

            var aioCellData = ObjectPool.Instance.Fetch<AIOCell>();
            aioCellData.CellId = cellId;
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
            aoiCell.AllUnitEntities[aoiUnitEntity.Id] = aoiUnitEntity;
            

            if (aoiUnitEntity.UETypeEnum == UETypeEnum.Player)
            {
                aoiCell.PlayerAOIEntities[aoiUnitEntity.CellId] = aoiUnitEntity;
                var AOICellIds = AOIHelper.GetAOICellIds(aoiUnitEntity.CellId);
                foreach (var AOICellId in AOICellIds)
                {
                    var AOICellData = aoiManagerComponent.GetCellData(AOICellId);
                    if (AOICellData != null)
                    {
                        aoiUnitEntity.PlayerSeeUnits(AOICellData.AllUnitEntities.Keys);
                    }
                }
            }
        }

        public static void UnBindUnitEntity(this AOIManagerComponent aoiManagerComponent, AOIUnitEntity aoiUnitEntity)
        {
            var aoiCell = aoiManagerComponent.GetCellData(aoiUnitEntity.CellId);
            if (aoiCell == null)
            {
                return;
            }

            aoiCell.AllUnitEntities.Remove(aoiUnitEntity.Id);
            if (aoiUnitEntity.UETypeEnum == UETypeEnum.Player)
            {
                var AOICellIds = AOIHelper.GetAOICellIds(aoiUnitEntity.CellId);
                aoiCell.PlayerAOIEntities.Remove(aoiUnitEntity.CellId);
                
                foreach (var AOICellId in AOICellIds)
                {
                    var AOICellData = aoiManagerComponent.GetCellData(AOICellId);
                    if (AOICellData != null)
                    {
                        aoiUnitEntity.PlayerLeaveUnits(AOICellData.AllUnitEntities.Keys);
                    }
                }
            }
        }

        public static void AwakeCellUnitEntity(this AOIManagerComponent aoiManagerComponent, AOIUnitEntity aoiUnitEntity, long toCellId)
        {
            aoiUnitEntity.CellId = AOIHelper.GetCellId(-1000, -1000);
            aoiManagerComponent.MoveCell(aoiUnitEntity, toCellId);
        }



        public static void MoveCell(this AOIManagerComponent aoiManagerComponent, AOIUnitEntity aoiUnitEntity, long newCellId)
        {
            if (aoiUnitEntity.CellId == newCellId)
            {
                return;
            }

            Log.Info($"MoveCell {newCellId}");
            
            var oldCellId = aoiUnitEntity.CellId;
            
            var (oldX, oldY) = AOIHelper.GetCellXY(oldCellId);
            var (newX, newY) = AOIHelper.GetCellXY(newCellId);

            aoiManagerComponent.UnBindUnitEntity(aoiUnitEntity);
            for (long x = oldX - GameConstant.AOIWatchCellRadius; x <= oldX + GameConstant.AOIWatchCellRadius; x++)
            {
                for (long y = oldY - GameConstant.AOIWatchCellRadius; y <= oldY + GameConstant.AOIWatchCellRadius; y++)
                {
                    if (x < newX - GameConstant.AOIWatchCellRadius || x > newX + GameConstant.AOIWatchCellRadius || y < newY - GameConstant.AOIWatchCellRadius || y > newY + GameConstant.AOIWatchCellRadius)
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

            for (long x = newX - GameConstant.AOIWatchCellRadius; x <= newX + GameConstant.AOIWatchCellRadius; x++)
            {
                for (long y = newY - GameConstant.AOIWatchCellRadius; y <= newY + GameConstant.AOIWatchCellRadius; y++)
                {
                    if (x < oldX - GameConstant.AOIWatchCellRadius || x > oldX + GameConstant.AOIWatchCellRadius || y < oldY - GameConstant.AOIWatchCellRadius || y > oldY + GameConstant.AOIWatchCellRadius)
                    {
                        long cellId = AOIHelper.GetCellId((int)x, (int)y);
                        var enterCellData = aoiManagerComponent.GetCellData(cellId);

                        if (enterCellData != null)
                        {
                            aoiManagerComponent.EnterCellScope(enterCellData, aoiUnitEntity);    
                        }
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
            for (long x = oldX - GameConstant.AOIWatchCellRadius; x <= oldX + GameConstant.AOIWatchCellRadius; x++)
            {
                for (long y = oldY - GameConstant.AOIWatchCellRadius; y <= oldY + GameConstant.AOIWatchCellRadius; y++)
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
        
        /*
        public static List<AOIUnitEntity> GetAllWatchUnitEntities(this AOIManagerComponent aoiManagerComponent, AOIUnitEntity aoiUnitEntity)
        {
            var centerCellId = aoiUnitEntity.CellId;
            var (newX, newY) = AOIHelper.GetCellXY(centerCellId);
            
            List<AOIUnitEntity> watchUnitEntities = new List<AOIUnitEntity>();
            for (long x = newX - GameConstant.AOIWatchCellRadius; x <= newX + GameConstant.AOIWatchCellRadius; x++)
            {
                for (long y = newY - GameConstant.AOIWatchCellRadius; y <= newY + GameConstant.AOIWatchCellRadius; y++)
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
        */
        

        public static void EnterCellScope(this AOIManagerComponent aoiManagerComponent, AIOCell targetCell, AOIUnitEntity aoiUnitEntity)
        {
            foreach (var playerAOIEntity in targetCell.PlayerAOIEntities.Values)
            {
                playerAOIEntity.PlayerSeeUnit(aoiUnitEntity.Id);
            }
        }

        public static void LeaveCellScope(this AOIManagerComponent aoiManagerComponent, AIOCell targetCell, AOIUnitEntity aoiUnitEntity)
        {
            foreach (var playerAOIEntity in targetCell.PlayerAOIEntities.Values)
            {
                playerAOIEntity.PlayerLeaveUnit(aoiUnitEntity.Id);
            }
        }
    }
}