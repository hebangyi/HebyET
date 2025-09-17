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
        }
    }
    
    
    // TODO 对象池
    public class SyncDirtyUnitEntity
    {
        public long InsId;
        // TODO 对象池
        public Dictionary<ushort, IUnitEntityElemData> DirtyElemDatas = new ();
    }
}

