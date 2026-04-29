namespace ET
{
    [UnitEntityLogic]
    public class SkillUpdateLogic : ILogicTickUpdate
    {
        public void OnTick(LogicWorld logicWorld)
        {
            // 执行buff
            var allSkillRuntimeList = logicWorld.GetEntityIdsWithDataType(typeof(SkillRuntimeData));
            foreach (UnitEntity unitEntity in allSkillRuntimeList)
            {
                var runtimeData = unitEntity.GetUnitEntityLogicElemData<SkillRuntimeData>();

                foreach (var skillData in runtimeData.RuntimeSkillDatas)
                {
                    SkillHelper.RunBuff(unitEntity, skillData);
                }

                // 移除已经用完的SkillData
                runtimeData.RuntimeSkillDatas.RemoveAll(x => x.BuffStackIndex >= x.SkillConfig.BuffStacks.Length);
            }
        }
    }
}