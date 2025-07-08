using System;
using System.Collections.Generic;

namespace ET
{
    [ChildOf(typeof(World))]
    public class UnitEntity : Entity, IAwake, IDestroy
    {
        public long InsId;
        // componentId 对应的 组件数据
        public Dictionary<ushort, IUnitEntityElemData> UnitEntityData;
        public Dictionary<Type, IUnitEntityLogicElemData> UnitEntityLogicData;
        

        public World World
        {
            get
            {
                return this.GetParent<World>();
            }
        }
    }
} 