namespace ET
{
    [ComponentOf(typeof(UnitEntity))]
    public class PlayerServerSkillComponent: Entity, IAwake
    {
        public UnitEntityPlayerSkillDataItem NormalAttackSkill;
    }
}
