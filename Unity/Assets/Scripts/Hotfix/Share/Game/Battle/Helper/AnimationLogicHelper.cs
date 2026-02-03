namespace ET
{
    public static class AnimationLogicHelper
    {
        public static bool ChangeAnimateStatus(this UnitEntity unitEntity, AnimateStateEnum animateState)
        {
            var unitEntityAnimation = unitEntity.GetUnitEntityElemData<UnitEntityAnimationStateData>();
            if (unitEntityAnimation == null)
            {
                return false;
            }

            if (unitEntityAnimation.AnimateState == animateState)
            {
                return false;
            }
            
            unitEntityAnimation.AnimateState = animateState;
            unitEntityAnimation.ActiveFrame = unitEntity.LogicWorld().Frame;
            return true;
        }

        public static bool ChangeUseSkillStatus(UnitEntity unitEntity, long skillId, uint skillActiveFrame)
        {
            var unitEntityAnimation = unitEntity.GetUnitEntityElemData<UnitEntityAnimationStateData>();
            if (unitEntityAnimation == null)
            {
                return false;
            }
            
            unitEntityAnimation.AnimateState = AnimateStateEnum.Skill;
            unitEntityAnimation.ActiveFrame = unitEntity.LogicWorld().Frame;

            var skillStateData = UnitEntityAnimationSkillStateData.Create();
            skillStateData.SkillId = skillId;
            skillStateData.ActiveFrame = skillActiveFrame;

            unitEntityAnimation.CurrentSkillStateData = skillStateData;
            return true;
        }

        public static void Stop(this UnitEntity unitEntity)
        {
            unitEntity.ChangeAnimateStatus(AnimateStateEnum.Idle);
        }
    }
}