using System.Collections.Generic;

namespace ET.Client
{
    [ChildOf]
    public class ClientUnitEntity: Entity, IAwake, IDestroy
    {
        public long InsId;
        // componentId 对应的 组件数据
        public Dictionary<ushort, IUnitEntityElemData> UnitEntityData;
    }
}

