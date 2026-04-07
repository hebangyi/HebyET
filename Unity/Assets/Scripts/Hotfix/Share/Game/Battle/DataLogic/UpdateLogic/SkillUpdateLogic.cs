namespace ET
{
    [UnitEntityLogic]
    public class SkillUpdateLogic : ILogicTickUpdate
    {
        public void OnTick(LogicWorld logicWorld)
        {
            foreach (UnitEntity unitEntity in logicWorld.GetEntityIdsWithDataType(typeof(SkillRuntimeData)))
            {
                var skillRuntimeData = unitEntity.GetUnitEntityLogicElemData<SkillRuntimeData>();
                foreach (var runtimeBuffData in skillRuntimeData.RunningBuffDatas)
                {
                    if (logicWorld.Frame >= runtimeBuffData.BuffEndFrame)
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
                        var checkFrame = FrameHelper.CalFrameNum(runtimeSkillData.SkillConfig.BuffOffSetSTime) + runtimeSkillData.SkillStartFrame;
                        // 释放Buff时间判断
                        if (logicWorld.Frame >= checkFrame)
                        {
                            runtimeSkillData.IsRunBuff = true;
                            // 技能加入执行队列
                            foreach (var buffData in runtimeSkillData.AllBuffDatas)
                            {
                                buffData.BuffStartFrame = checkFrame + buffData.BuffConfig.OffExecuteTime;
                                if (buffData.BuffConfig.DurationType == 0)
                                {
                                    buffData.BuffEndFrame = buffData.BuffStartFrame;
                                }
                                else if (buffData.BuffConfig.DurationType == 1)
                                {
                                    buffData.BuffEndFrame = buffData.BuffStartFrame + FrameHelper.CalFrameNum(buffData.BuffConfig.DurationTime);
                                }
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
                            if (buffData.BuffStartFrame > logicWorld.Frame)
                            {
                                continue;
                            }

                            buffData.IsRun = true;
                            BuffHelper.OnBuffEnter(unitEntity, buffData);
                            // 立即执行
                            if (buffData.BuffStartFrame == buffData.BuffEndFrame)
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
                        var animationFrame = FrameHelper.CalFrameNum(runtimeSkillData.SkillConfig.SkillAnimationSTime) + runtimeSkillData.SkillStartFrame;
                        // 释放Buff时间判断
                        if (logicWorld.Frame >= animationFrame)
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