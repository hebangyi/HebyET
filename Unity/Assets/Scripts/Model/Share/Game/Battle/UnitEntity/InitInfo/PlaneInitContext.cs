namespace ET
{
    // 地块生成上下文
    [ComponentOf(typeof(UnitEntity))]
    public class PlaneInitContext : Entity, IAwake
    {
        public PlantGenContext PlantGenContext;
    }
}