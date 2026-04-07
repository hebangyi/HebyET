namespace ET
{
    [UnitEntityLogic]
    public class UnitEntityTowardAngleClientInput : BaseLogicClientInput<UnitEntityTowardAngle>
    {
        public override bool CanInput(UnitEntity unitEntity, UnitEntityTowardAngle elementData)
        {
            return true;
        }

        public override void Updated(UnitEntity unitEntity)
        {
        }
    }
}

