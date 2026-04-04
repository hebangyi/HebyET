namespace ET
{
    [UnitEntityLogic]
    public class SkillUpdateLogic : ILogicTickUpdate
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

                    // 执行Buff
                    BuffHelper.OnBuffTick(unitEntity, runtimeBuffData);
                }

                // 移除已经执行完的Skill
                skillRuntimeData.RunningBuffDatas.RemoveAll(x => x.IsExit);

                foreach (var runtimeSkillData in skillRuntimeData.RuntimeSkillDatas)
                {
                    if (!runtimeSkillData.IsRunBuff)
                    {
                        var checkTime = runtimeSkillData.SkillConfig.BuffOffSetSTime + runtimeSkillData.SkillStartTime;
                        // 释放Buff时间判断
                        if (now >= checkTime)
                        {
                            runtimeSkillData.IsRunBuff = true;
                            // 技能加入执行队列
                            foreach (var buffData in runtimeSkillData.AllBuffDatas)
                            {
                                buffData.BuffStartTime = checkTime + buffData.OffExecuteTime;
                                buffData.BuffEndTime = buffData.BuffStartTime + buffData.BuffConfig.DurationTime; // Buff持续时间
                            }
                        }
                    }

                    // 将所有没有执行的Buff加入执行队列
                    if (runtimeSkillData.IsRunBuff)
                    {
                        foreach (var buffData in runtimeSkillData.AllBuffDatas)
                        {
                            if (buffData.IsRun)
                            {
                                continue;
                            }

                            // 没到Buff执行时间
                            if (buffData.BuffStartTime > now)
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
                    

                    // 技能动画
                    if (!runtimeSkillData.IsRunAnimation)
                    {
                        var checkTime = runtimeSkillData.SkillConfig.SkillAnimationSTime + runtimeSkillData.SkillStartTime;
                        // 释放Buff时间判断
                        if (now >= checkTime)
                        {
                            // 设置动画
                            AnimationLogicHelper.ChangeUseSkillStatus(unitEntity, runtimeSkillData.SkillConfig.Id, logicWorld.Frame);
                            runtimeSkillData.IsRunAnimation = true;
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