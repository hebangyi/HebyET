using System.Collections.Generic;

namespace ET
{
    public class World: Entity
    {
        public Dictionary<long, UnitEntity> AllEntity = new Dictionary<long, UnitEntity>();
    }
}