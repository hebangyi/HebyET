namespace ET
{
    [UnitEntityLogic]
    public class UnitEntityAnimationClientInput : BaseLogicClientInput<UnitEntityAnimationStateData>
    {
        public override bool CanInput(UnitEntityAnimationStateData elementData)
        {
            return true;
        }

        public override void Updated(UnitEntity unitEntity)
        {
        }
    }
}

