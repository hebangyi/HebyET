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
            var unitEntityBloodData = target.GetUnitEntityElemData<UnitEntityBloodData>();
            if (unitEntityBloodData == null)
            {
                return;
            }
            
            unitEntityBloodData.CurrentBlood -= 1;
            if (unitEntityBloodData.CurrentBlood < 0)
            {
                unitEntityBloodData.CurrentBlood = 0;
            }
        }
    }
}

