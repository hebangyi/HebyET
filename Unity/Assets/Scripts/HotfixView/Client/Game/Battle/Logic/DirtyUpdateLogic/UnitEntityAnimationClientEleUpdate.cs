namespace ET.Client
{
    [UnitEntityViewLogic]
    public class UnitEntityAnimationClientEleUpdate : BaseClientEleLogic<UnitEntityAnimation>
    {
        public override void OnInit(UnitEntity unitEntity)
        {
        }

        public override void OnDestroy(UnitEntity unitEntity)
        {
        }

        public override void OnUpdateT(UnitEntity unitEntity, UnitEntityAnimation oldData, UnitEntityAnimation newData)
        {
            var spineAnimation = unitEntity.GetSpineAnimation();
            if (spineAnimation == null)
            {
                return;
            }
            
            switch (newData.AnimateState)
            {
                case AnimateStateEnum.Idle:
                    unitEntity.GetComponent<UnitEntitySpineAnimationComponent>().SetAnimationAtTime("idle", 0);
                    break;
                case AnimateStateEnum.Walk:
                    unitEntity.GetComponent<UnitEntitySpineAnimationComponent>().SetAnimationAtTime("walk", 0);
                    break;
                case AnimateStateEnum.Run:
                    unitEntity.GetComponent<UnitEntitySpineAnimationComponent>().SetAnimationAtTime("run", 0);
                    break;
                case AnimateStateEnum.Attack:
                    unitEntity.GetComponent<UnitEntitySpineAnimationComponent>().SetAnimationAtTime("skill_1", 0, false);
                    break;
            }
        }
    }
}