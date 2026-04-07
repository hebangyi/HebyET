namespace ET.Client
{
    public static class ClientBuffHelper
    {
        public static bool IsRigidity(ClientUnitEntity unitEntity)
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
