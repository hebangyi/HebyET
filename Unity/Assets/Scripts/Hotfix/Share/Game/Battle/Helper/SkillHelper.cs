namespace ET
{
    public static class SkillHelper
    {
        public static void UseSkill(UnitEntity unitEntity, long skillCId)
        {
            var skillConfig = SkillConfigCategory.Instance.GetById(skillCId);
            if (skillConfig == null)
            {
                Log.Error($"技能使用异常 找不到配置ID {skillCId}");
                return;
            }

            var skillRuntimeData = unitEntity.GetUnitEntityLogicElemData<SkillRuntimeData>();

            SkillData skillData = new SkillData();
            skillData.UnitInsId = unitEntity.InsId;
            skillData.SkillConfig = skillConfig;
            
            // 创建BuffData
            foreach (var buffStack in skillConfig.BuffStacks)
            {
                BuffConfig buffConfig = BuffConfigCategory.Instance.GetById(buffStack.BuffId);
                if (buffConfig == null)
                {
                    Log.Error($"使用技能{skillCId}异常, 找不到BuffID {buffStack.BuffId}");
                    continue;
                }
                
                BuffData buffData = new BuffData();
                buffData.BuffId = skillRuntimeData.BuffIdGen++;
                buffData.BuffConfig = buffConfig;
                buffData.SkillData = skillData;
                buffData.OffExecuteTime = buffStack.StartTime;
                
                skillData.AllBuffDatas.Add(buffData);
            }
            
            // 加入正在执行的技能
            skillRuntimeData.RuntimeSkillDatas.Add(skillData);
            
        }
    }    
}
