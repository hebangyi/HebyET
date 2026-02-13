namespace ET.Client
{
    [UnitEntityViewLogic]
    public class UnitEntityBloodUpdateLogic: BaseClientEleLogic<UnitEntityBloodData>
    {
        public override void OnInitT(UnitEntity unitEntity, UnitEntityBloodData elemData)
        {
        }

        public override void OnDestroyT(UnitEntity unitEntity, UnitEntityBloodData elemData)
        {
        }

        public override void OnUpdateT(UnitEntity unitEntity, UnitEntityBloodData oldData, UnitEntityBloodData newData)
        {
            unitEntity.GetComponent<UnitEntityHealthBarComponent>()?.RefreashBar();
        }
    }
}

