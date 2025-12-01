namespace ET
{
    [ComponentOf(typeof(UnitEntity))]
    public class PlayerInitContext: Entity, IAwake
    {
        public long PlayerId;
    }
}

