using System.Collections.Generic;
using System.ComponentModel;

namespace ET.Client
{
    [ComponentOf(typeof(UnitEntity))]
    public class PlayerClientSkillComponent: Entity, IAwake
    {
        public UnitEntityPlayerSkillDataItem NormalAttackSkill;
    }
}