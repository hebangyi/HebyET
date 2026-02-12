namespace ET
{
    [UnitEntityLogic]
    public class SkillUpdateLogic: ILogicTickUpdate
    {
        public void OnTick(LogicWorld logicWorld)
        {
         
            var now = logicWorld.NowMilliTime;
            foreach (UnitEntity unitEntity in logicWorld.GetEntityIdsWithDataType(typeof(SkillRuntimeData)))
            {
                var skillRuntimeData = unitEntity.GetUnitEntityLogicElemData<SkillRuntimeData>();
                foreach (var runtimeBuffData in skillRuntimeData.RunningBuffDatas)
                {
                    if (now > runtimeBuffData.BuffEndTime)
                    {
                        BuffHelper.OnBuffExit(unitEntity, runtimeBuffData);
                        continue;
                    }
                    
                    BuffHelper.OnBuffInterrupt(unitEntity, runtimeBuffData);
                }

                // 移除已经执行完的Skill
                skillRuntimeData.RunningBuffDatas.RemoveAll(x => x.IsExit);
                
                
                foreach (var runtimeSkillData in skillRuntimeData.RuntimeSkillDatas)
                {
                    if (!runtimeSkillData.IsRun)
                    {
                        runtimeSkillData.IsRun = true;
                        runtimeSkillData.SkillStartTime = now;
                        
                        foreach (var allBuffData in runtimeSkillData.AllBuffDatas)
                        {
                            allBuffData.BuffStartTime = now + allBuffData.OffExecuteTime;
                            allBuffData.BuffEndTime = allBuffData.BuffStartTime + allBuffData.BuffConfig.DurationTime; // Buff持续时间
                        }
                    }
                    
                    // 将所有没有执行的Buff加入执行队列
                    foreach (var buffData in runtimeSkillData.AllBuffDatas)
                    {
                        if (buffData.IsRun)
                        {
                            continue;
                        }

                        // 没到Buff执行时间
                        if (buffData.BuffStartTime < now)
                        {
                            continue;
                        }
                        
                        buffData.IsRun = true;
                        BuffHelper.OnBuffEnter(unitEntity, buffData);
                        // 立即执行
                        if (buffData.BuffStartTime == buffData.BuffEndTime)
                        {
                            BuffHelper.OnBuffExit(unitEntity, buffData);
                        }
                        else
                        {
                            skillRuntimeData.RunningBuffDatas.Add(buffData);
                        }
                    }
                }
                
                // 技能Tick
                // 移除已经执行完的BuffData
                skillRuntimeData.RuntimeSkillDatas.RemoveAll(x => x.AllBuffDatas.Count == x.ExecutedBuffCount);
            }
        }
    }
}

