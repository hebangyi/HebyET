using System.Collections.Generic;

namespace ET
{
    public static class BuffHelper
    {
        public static void OnBuffEnter(UnitEntity unitEntity, BuffData buffData)
        {
            var buffExecutor = BuffManagerComponent.Instance.BuffType2BuffConfigs.GetValueOrDefault(buffData.BuffConfig.BuffType);
            if (buffExecutor == null)
            {
                Log.Warning($"BuffConfig {buffData.BuffConfig.Id} 没有找到执行器!");
                return;
            }
            
            buffExecutor.Enter(unitEntity, buffData);
        }


        public static void OnBuffTick(UnitEntity unitEntity, BuffData buffData)
        {
            var buffExecutor = BuffManagerComponent.Instance.Tick2BuffConfigs.GetValueOrDefault(buffData.BuffConfig.BuffType);
            if (buffExecutor == null)
            {
                return;
            }
            
            buffExecutor.Tick(unitEntity, buffData);
        }

        public static void OnBuffInterrupt(UnitEntity unitEntity, BuffData buffData)
        {
            var buffExecutor = BuffManagerComponent.Instance.Tick2BuffConfigs.GetValueOrDefault(buffData.BuffConfig.BuffType);
            if (buffExecutor == null)
            {
                return;
            }
            
            buffExecutor.Interrupt(unitEntity, buffData);
        }

        public static void OnBuffExit(UnitEntity unitEntity, BuffData buffData)
        {
            buffData.SkillData.ExecutedBuffCount++;
            buffData.IsExit = true;
            
            var buffExecutor = BuffManagerComponent.Instance.BuffType2BuffConfigs.GetValueOrDefault(buffData.BuffConfig.BuffType);
            if (buffExecutor == null)
            {
                Log.Warning($"BuffConfig {buffData.BuffConfig.Id} 没有找到执行器!");
                return;
            }
            
            buffExecutor.Exit(unitEntity, buffData);
        }
    }
}