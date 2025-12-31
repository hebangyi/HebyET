namespace ET.Client
{
    [UnitEntityViewLogic]
    public class UnitEntityAnimationClientEleUpdate : IClientEleUpdate
    {
        public ushort WatchComponentId()
        {
            return OpcodeType.Instance.GetOpcode(typeof(UnitEntityAnimation));
        }

        public void OnUpdate(UnitEntity unitEntity, IUnitEntityElemData oldData, IUnitEntityElemData newData)
        {
            var spineAnimation = unitEntity.GetSpineAnimation();
            if (spineAnimation == null)
            {
                return;
            }
            
            var unitEntityAnimation = newData as UnitEntityAnimation;
            AnimateStateEnum animateState = unitEntityAnimation.AnimateState;
            switch (animateState)
            {
                case AnimateStateEnum.Idle:
                    spineAnimation.state.SetAnimation(0, "idle", true);
                    break;
                case AnimateStateEnum.Walk:
                    spineAnimation.state.SetAnimation(0, "walk", true);
                    break;
                case AnimateStateEnum.Run:
                    spineAnimation.state.SetAnimation(0, "run", true);
                    break;
                case AnimateStateEnum.Attack:
                    spineAnimation.state.SetAnimation(0, "skill_1", true);
                    break;
            }
        }
    }
}