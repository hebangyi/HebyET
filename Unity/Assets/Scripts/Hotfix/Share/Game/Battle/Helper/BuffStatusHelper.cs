using System.Collections.Generic;

namespace ET
{
    public static class BuffStatusHelper
    {
        public static void AddBuffStatus(UnitEntity unitEntity, BuffData buffData)
        {
            var unitEntityBuffData = unitEntity.GetUnitEntityElemData<UnitEntityBuffData>();
            
            // buffData.buff
        }
        
        public static bool IsRigidity(UnitEntity unitEntity)
        {
            var unitEntityBuffData = unitEntity.GetUnitEntityElemData<UnitEntityBuffData>();
            if (unitEntityBuffData == null)
            {
                return false;
            }

            var dataItem = unitEntityBuffData.BuffStatus2DataItems.GetValueOrDefault(BuffStatus.Rigidity);

            if (dataItem == null)
            {
                return false;
            }

            if (unitEntity.LogicWorld().Frame < dataItem.EndFrame)
            {
                return true;
            }

            return false;
        }
    }
}

