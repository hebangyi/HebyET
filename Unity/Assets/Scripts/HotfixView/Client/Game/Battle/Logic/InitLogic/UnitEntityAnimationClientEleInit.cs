namespace ET.Client
{
    [UnitEntityViewLogic]
    public class UnitEntityAnimationClientEleInit : IClientEleInit
    {
        public ushort WatchComponentId()
        {
            return OpcodeType.Instance.GetOpcode(typeof(UnitEntityAnimation));
        }

        public void OnInit(UnitEntity unitEntity)
        {
            var spineAnimation = unitEntity.GetSpineAnimation();
            if (spineAnimation == null)
            {
                return;
            }
            
            var unitEntityAnimation = unitEntity.GetUnitEntityElemData<UnitEntityAnimation>();

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

        public void OnDestroy(UnitEntity unitEntity)
        {
        }
    }
}
