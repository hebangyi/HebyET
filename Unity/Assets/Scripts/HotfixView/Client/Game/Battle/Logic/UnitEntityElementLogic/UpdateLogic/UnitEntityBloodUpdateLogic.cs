namespace ET.Client
{
    [UnitEntityViewLogic]
    public class UnitEntityBloodUpdateLogic: BaseClientEleLogic<UnitEntityBloodData>
    {
        public override void OnInitT(ClientUnitEntity unitEntity, UnitEntityBloodData elemData)
        {
        }

        public override void OnDestroyT(ClientUnitEntity unitEntity, UnitEntityBloodData elemData)
        {
        }

        public override void OnUpdateT(ClientUnitEntity unitEntity, UnitEntityBloodData oldData, UnitEntityBloodData newData)
        {
            unitEntity.GetComponent<UnitEntityHealthBarComponent>()?.RefreashBar();
        }
    }
}

