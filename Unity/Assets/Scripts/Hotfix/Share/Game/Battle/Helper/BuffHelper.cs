using System.Collections.Generic;

namespace ET
{
    public static class BuffHelper
    {
        public static void OnBuffHandlerEnter(UnitEntity unitEntity, BuffData buffData)
        {
            var buffExecutor = BuffManagerComponent.Instance.BuffType2BuffConfigs.GetValueOrDefault(buffData.BuffConfig.BuffHandler);
            if (buffExecutor == null)
            {
                Log.Warning($"BuffConfig {buffData.BuffConfig.Id} 没有找到执行器!");
                return;
            }
            
            buffExecutor.Enter(unitEntity, buffData);
        }

        public static void OnBuffHandlerExit(UnitEntity unitEntity, BuffData buffData)
        {
            var buffExecutor = BuffManagerComponent.Instance.BuffType2BuffConfigs.GetValueOrDefault(buffData.BuffConfig.BuffHandler);
            if (buffExecutor == null)
            {
                Log.Warning($"BuffConfig {buffData.BuffConfig.Id} 没有找到执行器!");
                return;
            }
            
            
            buffExecutor.Exit(unitEntity, buffData);
        }
        
        


    }
}