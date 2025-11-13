namespace ET
{
    [UnitEntityLogic]
    public class PlayerUnitEntityPlayerAnimateStatusClientInput : BaseLogicClientInput<UnitEntityPlayerAnimateStatus>
    {
        public override bool CanInput(UnitEntityPlayerAnimateStatus elementData)
        {
            return true;
        }

        public override void Updated(UnitEntity unitEntity)
        {
        }
    }
}

