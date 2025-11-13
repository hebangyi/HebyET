namespace ET
{
    [UnitEntityLogic]
    public class PlayerUnitEntityPositionClientInput : BaseLogicClientInput<UnitEntityPosition>
    {
        public override bool CanInput(UnitEntityPosition elementData)
        {
            return true;
        }

        public override void Updated(UnitEntity unitEntity)
        {
        }
    }
}

