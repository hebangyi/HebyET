using System.Collections.Generic;

namespace ET.Client
{
    public static class ClientBuffStatusHelper
    {
    
        public static bool IsRigidity(ClientUnitEntity unitEntity)
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

            if (unitEntity.ClientWorld().Frame < dataItem.EndFrame)
            {
                return true;
            }

            return false;
        }
    }
}

