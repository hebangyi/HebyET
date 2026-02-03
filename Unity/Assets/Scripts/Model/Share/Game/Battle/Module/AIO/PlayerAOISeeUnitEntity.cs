using System;
using System.Collections.Generic;
using Unity.Mathematics;

namespace ET
{
    [ComponentOf(typeof(UnitEntity))]
    public class PlayerAOISeeUnitEntity: Entity, IAwake
    {
        public List<long> EnterEntityIds = new();
        public List<long> LeaveEntityIds = new();
        public HashSet<long> ManageEntityIds = new();
    }
}

