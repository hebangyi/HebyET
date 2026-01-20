namespace ET
{
    public static class AnimationLogicHelper
    {
        public static void ChangeAnimateStatus(this UnitEntity unitEntity, AnimateStateEnum animateState)
        {
            var unitEntityAnimation = unitEntity.GetUnitEntityElemData<UnitEntityAnimationStateData>();
            unitEntityAnimation.AnimateState = animateState;
            unitEntityAnimation.CurrentStateStartFrame = unitEntity.LogicWorld().Frame;
        }
    }
}