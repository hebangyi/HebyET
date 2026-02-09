namespace ET.Client
{
    public static class UnitEntityHelper
    {
        public static ClientWorld ClientWorld(this UnitEntity unitEntity)
        {
            return unitEntity.GetParent<ClientWorld>();
        }

        public static bool HasBloodNumerical(this UnitEntity unitEntity)
        {
            if (!unitEntity.HasUnitEntityElementData<UnitEntityCommonData>() || !unitEntity.HasUnitEntityElementData<UnitEntityBloodData>())
            {
                return false;
            }

            if (!unitEntity.GetUnitEntityElemData<UnitEntityCommonData>().NumericalDatas.ContainsKey(UnitEntityNumericalTypeEnum.Blood))
            {
                return false;
            }

            return true;
        }
        
    }    
}
