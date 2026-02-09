using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(LogicWorld))]
    public class WorldSkillExecuteComponent : Entity, IAwake
    {
        public Dictionary<long, UnitEntitySkill> InsId2UnitEntitySkills = new Dictionary<long, UnitEntitySkill>();
    }

    public class UnitEntitySkill
    {
        public long InsId;
        public List<UnitEntitySkillItem> UnitEntitySkillItems = new ();
    }

    public class UnitEntitySkillItem
    {
        public long SkillId;
        public long StartTime;
    }
}