using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(UnitEntity))]
    public class ClientSkillComponent: Entity, IAwake
    {
        public Dictionary<long, long> SkillCDS = new();
    }
}
