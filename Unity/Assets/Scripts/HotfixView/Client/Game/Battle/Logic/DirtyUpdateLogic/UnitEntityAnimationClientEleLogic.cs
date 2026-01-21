namespace ET.Client
{
    [UnitEntityViewLogic]
    public class UnitEntityAnimationClientEleLogic : BaseClientEleLogic<UnitEntityAnimationStateData>
    {
        public override void OnInitT(UnitEntity unitEntity, UnitEntityAnimationStateData elemData)
        {
            PlayAnimation(unitEntity, elemData);
        }

        public override void OnDestroyT(UnitEntity unitEntity, UnitEntityAnimationStateData elemData)
        {
        }

        public override void OnUpdateT(UnitEntity unitEntity, UnitEntityAnimationStateData oldData, UnitEntityAnimationStateData newData)
        {
      
            PlayAnimation(unitEntity, newData);
        }


        public void PlayAnimation(UnitEntity unitEntity, UnitEntityAnimationStateData animationStateData)
        {
            var spineAnimation = unitEntity.GetSpineAnimation();
            if (spineAnimation == null)
            {
                return;
            }
            
            
            var animateStateEnum = animationStateData.AnimateState;
            var startFrame = animationStateData.CurrentStateStartFrame;
            var clientWorld = unitEntity.ClientWorld();

            uint subFrame = 0;
            if (clientWorld.Frame > startFrame)
            {
                subFrame = clientWorld.Frame - startFrame;
            }
            
            float subTime = subFrame * clientWorld.LogicInterval * 1.0f / 1000;
            if (animateStateEnum == AnimateStateEnum.Skill)
            {
                
                // TODO 播放技能动画
                unitEntity.GetComponent<UnitEntitySpineAnimationComponent>().SetAnimationAtTime("skill_1", subTime, false);
                return;
            }
            
            // 常规状态
            switch (animateStateEnum)
            {
                case AnimateStateEnum.Idle:
                    unitEntity.GetComponent<UnitEntitySpineAnimationComponent>().SetAnimationAtTime("idle", subTime, true);
                    break;
                case AnimateStateEnum.Walk:
                    unitEntity.GetComponent<UnitEntitySpineAnimationComponent>().SetAnimationAtTime("walk", subTime, true);
                    break;
                case AnimateStateEnum.Run:
                    unitEntity.GetComponent<UnitEntitySpineAnimationComponent>().SetAnimationAtTime("run", subTime, true);
                    break;
            }
        }
        
        
    }
}