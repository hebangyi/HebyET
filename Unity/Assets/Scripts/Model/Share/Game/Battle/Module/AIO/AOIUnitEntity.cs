using System;
using System.Collections.Generic;
using Unity.Mathematics;

namespace ET
{
    [ComponentOf(typeof(UnitEntity))]
    public class AOIUnitEntity : Entity, IAwake<float2, UETypeEnum>, IDestroy
    {
        public long CellId;

        public UETypeEnum UETypeEnum { get; set; }
    }
}