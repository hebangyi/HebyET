namespace ET
{
    public static class AnimationHelper
    {
        public static void ChangeAnimateStatus(this UnitEntity unitEntity, AnimateStateEnum animateStatus)
        {
            var unitEntityAnimation = unitEntity.GetUnitEntityElemData<UnitEntityAnimation>();
            unitEntityAnimation.AnimateStatus = animateStatus;
        }
    }
}