namespace ET
{
    [UnitEntityLogic]
    public class PlayerUnitEntityCameraDataClientInput: BaseLogicClientInput<UnitEntityCameraData>
    {
        public override bool CanInput(UnitEntityCameraData elementData)
        {
            return true;
        }

        public override void Updated(UnitEntity unitEntity)
        {
        }
    }    
}
