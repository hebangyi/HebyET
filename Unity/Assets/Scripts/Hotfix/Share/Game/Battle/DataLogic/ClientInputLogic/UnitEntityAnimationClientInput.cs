namespace ET
{
    [UnitEntityLogic]
    public class UnitEntityAnimationClientInput : BaseLogicClientInput<UnitEntityAnimation>
    {
        public override bool CanInput(UnitEntityAnimation elementData)
        {
            return true;
        }

        public override void Updated(UnitEntity unitEntity)
        {
        }
    }
}

