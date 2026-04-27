using FairyGUI;

namespace ET.Client
{
    public static class UnitEntityCommonDataHelper
    {
        public static UETypeEnum UnitEntityType(this ClientUnitEntity unitEntity)
        {
            var unitEntityCommonData = unitEntity.GetUnitEntityElemData<UnitEntityCommonData>();
            if (unitEntityCommonData == null)
            {
                return ET.UETypeEnum.None;
            }

            return unitEntityCommonData.UnitEntityType;
        }
        
        public static bool HasBloodNumericalData(this ClientUnitEntity unitEntity)
        {
            if (!unitEntity.HasUnitEntityElementData<UnitEntityCommonData>() || !unitEntity.HasUnitEntityElementData<UnitEntityCurrentNumericalData>())
            {
                return false;
            }

            if (!unitEntity.GetUnitEntityElemData<UnitEntityCommonData>().NumericalDatas.ContainsKey(UnitEntityNumericalTypeEnum.Blood))
            {
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// 同步血量数据到血量条
        /// </summary>
        /// <param name="unitEntity"></param>
        public static async ETTask AddData2HealthBar(this ClientUnitEntity unitEntity)
        {
            if (unitEntity.HasBloodNumericalData())
            {
                // 血量条组件
                var gObject = await FGUIComponent.Instance.CreateGObject(FGUIPackage.PKG_Battle, FGUIResName.RES_Battle_FGUIHealthBar);
                unitEntity.AddComponent<UnitEntityHealthBarComponent, GObject>(gObject);
            }
        }
    }
}

