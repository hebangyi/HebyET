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
            unitEntityAnimation.CurrentStateStartFrame = unitEntity.LogicWorld().Frame;
            return true;
        }

        public static void Stop(this UnitEntity unitEntity)
        {
            unitEntity.ChangeAnimateStatus(AnimateStateEnum.Idle);
        }
    }
}