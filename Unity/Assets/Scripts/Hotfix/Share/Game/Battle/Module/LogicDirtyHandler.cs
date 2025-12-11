using System.Collections.Generic;
using System.Linq;

namespace ET
{
    public class LogicDirtyHandler : IDirtyHandler
    {
        private LogicWorld mLogicWorld;

        public LogicDirtyHandler(LogicWorld logicWorld)
        {
            this.mLogicWorld = logicWorld;
        }

        public void Dirty(long insId, IUnitEntityElemData elemData)
        {
            var dirtyUnitEntity = this.mLogicWorld.DirtyUnitEntities.GetValueOrDefault(insId);
            if (dirtyUnitEntity == null)
            {
                dirtyUnitEntity = new SyncDirtyUnitEntity();
                dirtyUnitEntity.InsId = insId;
                this.mLogicWorld.DirtyUnitEntities[insId] = dirtyUnitEntity;
            }

            ushort compId = OpcodeType.Instance.GetOpcode(elemData.GetType());
            dirtyUnitEntity.DirtyElemDatas[compId] = elemData;
            
            if (elemData.GetType() == typeof(UnitEntityPosition))
            {
                var unitEntity = this.mLogicWorld.AllEntities.GetValueOrDefault(insId);
                if (unitEntity != null && elemData is UnitEntityPosition unitEntityPosition)
                {
                    var newCellId = AOIHelper.GetCellId(unitEntityPosition.Position);
                    var aoiUnitEntity = unitEntity.GetComponent<AOIUnitEntity>();
                    if (aoiUnitEntity != null && newCellId != aoiUnitEntity.CellId)
                    {
                        var aoiManagerComponent = this.mLogicWorld.GetComponent<AOIManagerComponent>();
                        aoiManagerComponent.MoveCell(aoiUnitEntity, newCellId);
                    }
                }
            }
        }
    }
}

