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
            
            // 创建BuffData
            foreach (var buffStack in skillConfig.BuffStacks)
            {
                BuffConfig buffConfig = BuffConfigCategory.Instance.GetById(buffStack.BuffId);
                if (buffConfig == null)
                {
                    Log.Error($"使用技能 {skillConfig.Id} 异常, 找不到BuffID {buffStack.BuffId}");
                    continue;
                }
                
                BuffData buffData = new BuffData();
                buffData.BuffId = buffConfig.Id;
                buffData.BuffConfig = buffConfig;
                buffData.SkillData = skillData;
                
                skillData.AllBuffDatas.Add(buffData);
            }
            
            // 加入正在执行的技能
            skillRuntimeData.RuntimeSkillDatas.Add(skillData);
        }
    }    
}
