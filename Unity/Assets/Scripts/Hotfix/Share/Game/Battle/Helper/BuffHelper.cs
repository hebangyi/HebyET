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
            
            unitEntity.GetComponent<BuffComponent>().AddBuff(buffData.BuffId);
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
            
            
            unitEntity.GetComponent<BuffComponent>().RemoveBuff(buffData.BuffId);
            buffExecutor.Exit(unitEntity, buffData);
        }
        
        
        public static bool IsRigidity(UnitEntity unitEntity)
        {
            var unitEntityBuffData = unitEntity.GetUnitEntityElemData<UnitEntityBuffData>();
            if (unitEntityBuffData == null)
            {
                return false;
            }

            foreach (var buffId2BuffDataItem in unitEntityBuffData.BuffId2BuffDataItems)
            {
                var buffConfig = BuffConfigCategory.Instance.GetById(buffId2BuffDataItem.Key);
                if (buffConfig == null)
                {
                    continue;
                }

                if (buffConfig.BuffType == BuffTypeEnum.Rigidity)
                {
                    return true;
                }
            }

            return false;
        }

    }
}