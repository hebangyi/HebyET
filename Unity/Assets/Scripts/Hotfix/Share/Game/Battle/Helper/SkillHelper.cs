namespace ET
{
    public static class SkillHelper
    {
        public static void UseSkill(UnitEntity unitEntity, SkillConfig skillConfig)
        {
            var skillRuntimeData = unitEntity.GetUnitEntityLogicElemData<SkillRuntimeData>();
            // 创建技能信息
            // TODO 使用缓存 防止 new 重复使用
            SkillData skillData = new SkillData();
            skillData.UnitInsId = unitEntity.InsId;
            skillData.SkillConfig = skillConfig;
            skillData.SkillStartFrame = unitEntity.LogicWorld().Frame;
            skillData.BuffStackIndex = 0;
            skillData.BuffStackStartFrame = skillData.SkillStartFrame + FrameHelper.CalFrameNum(skillConfig.BuffStackStartTime);
            
            
            // 加入正在执行的技能
            skillRuntimeData.RuntimeSkillDatas.Add(skillData);
            
            // 动画
            AnimationLogicHelper.ChangeUseSkillStatus(unitEntity, skillConfig.Id, unitEntity.LogicWorld().Frame);
            
            // 立即执行Buff
            RunBuff(unitEntity, skillData);
        }

        public static void RunBuff(UnitEntity unitEntity, SkillData skillData)
        {
            var currentFrame = unitEntity.LogicWorld().Frame;
            var skillRuntimeData = unitEntity.GetUnitEntityLogicElemData<SkillRuntimeData>();
            if (skillRuntimeData == null)
            {
                return;
            }

            while (skillData.BuffStackIndex < skillData.SkillConfig.BuffStacks.Length)
            {
                var buffStack = skillData.SkillConfig.BuffStacks[skillData.BuffStackIndex];
                var buffStartFrame = skillData.BuffStackStartFrame + FrameHelper.CalFrameNum(buffStack.StartTime);
                if (buffStartFrame < currentFrame)
                {
                    return;
                }
                
                BuffConfig buffConfig = BuffConfigCategory.Instance.GetById(buffStack.BuffId);
                if (buffConfig != null)
                {
                    // 执行Buff
                    // TODO new
                    BuffData buffData = new BuffData();
                    buffData.BuffId = buffConfig.Id;
                    buffData.BuffConfig = buffConfig;
                    buffData.SkillData = skillData;
                    buffData.BuffStartFrame = buffStartFrame;
                    // 
                    BuffHelper.OnBuffHandlerEnter(unitEntity, buffData);
                    BuffHelper.OnBuffHandlerExit(unitEntity, buffData);
                }

                skillData.BuffStackIndex++;
            }
        }
    }    
}
