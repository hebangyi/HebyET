namespace ET
{
    public static class AnimationLogicHelper
    {
        public static void ChangeAnimateStatus(this UnitEntity unitEntity, AnimateStateEnum animateState)
        {
            var unitEntityAnimation = unitEntity.GetUnitEntityElemData<UnitEntityAnimation>();
            unitEntityAnimation.AnimateState = animateState;
        }
    }
}