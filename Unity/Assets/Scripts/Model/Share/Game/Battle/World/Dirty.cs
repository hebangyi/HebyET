using System.Collections.Generic;
using System.Linq;

namespace ET
{
    public class LogicDirtyHandler : IDirtyHandler
    {
        private World m_World;

        public LogicDirtyHandler(World world)
        {
            this.m_World = world;
        }

        public void Dirty(long insId, IUnitEntityElemData elemData)
        {
            var dirtyUnitEntity = m_World.DirtyUnitEntities.GetValueOrDefault(insId);
            if (dirtyUnitEntity == null)
            {
                dirtyUnitEntity = new SyncDirtyUnitEntity();
                dirtyUnitEntity.InsId = insId;
                m_World.DirtyUnitEntities[insId] = dirtyUnitEntity;
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

