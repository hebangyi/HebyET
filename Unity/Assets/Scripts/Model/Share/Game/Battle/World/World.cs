using System.Collections.Generic;

namespace ET
{
    public class World : Entity, IAwake
    {
        public Dictionary<long, UnitEntity> AllEntity = new ();
    }
}