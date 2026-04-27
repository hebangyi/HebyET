using System.Collections.Generic;

namespace ET.Client
{
    [UnitEntityViewLogic]
    public class UnitEntityCurrentNumericalDataUpdateLogic: BaseClientEleLogic<UnitEntityCurrentNumericalData>
    {
        public override void OnInitT(ClientUnitEntity unitEntity, UnitEntityCurrentNumericalData elemData)
        {
        }

        public override void OnDestroyT(ClientUnitEntity unitEntity, UnitEntityCurrentNumericalData elemData)
        {
        }

        public override void OnUpdateT(ClientUnitEntity unitEntity, UnitEntityCurrentNumericalData oldData, UnitEntityCurrentNumericalData newData)
        {
            if (oldData.NumericalDatas.GetValueOrDefault(UnitEntityNumericalTypeEnum.Blood) !=
                newData.NumericalDatas.GetValueOrDefault(UnitEntityNumericalTypeEnum.Blood))
            {
                unitEntity.GetComponent<UnitEntityHealthBarComponent>()?.RefreashBar();
            }
        }
    }
}

