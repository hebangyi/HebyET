using System;
using System.Collections.Generic;
using ET.Client;

namespace ET
{
    [ChildOf]
    public class UnitEntity : Entity, IAwake, IDestroy
    {
        public long InsId;
        // componentId 对应的 组件数据
        public Dictionary<ushort, IUnitEntityElemData> UnitEntityData;
        public Dictionary<Type, IUnitEntityLogicElemData> UnitEntityLogicData;
        

        /*public ClientWorld ClientWorld
        {
            get
            {
                return this.GetParent<ClientWorld>();
            }
        }*/

    }
} 