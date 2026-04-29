using System.Collections.Generic;

namespace ET
{
    public static class BuffLogicHelper
    {
        /// <summary>
        /// 造成伤害
        /// </summary>
        /// <param name="self">伤害的源</param>
        /// <param name="target">伤害的目标</param>
        public static void Damage(UnitEntity self, UnitEntity target)
        {
            var unitEntityBloodData = target.GetUnitEntityElemData<UnitEntityCurrentNumericalData>();
            if (unitEntityBloodData == null || !unitEntityBloodData.NumericalDatas.ContainsKey(UnitEntityNumericalTypeEnum.Blood))
            {
                return;
            }

            var currentBlood = unitEntityBloodData.NumericalDatas.GetValueOrDefault(UnitEntityNumericalTypeEnum.Blood);
            currentBlood -= 1;
            if (currentBlood < 0)
            {
                currentBlood = 0;
            }
            
            unitEntityBloodData.NumericalDatas[UnitEntityNumericalTypeEnum.Blood] = currentBlood;
            unitEntityBloodData.Dirty();
        }
    }
}

