namespace ET
{
    // 地块生成上下文
    [ComponentOf(typeof(UnitEntity))]
    public class UnitEntityInitContext : Entity, IAwake
    {
        public object Params { get; set; }
    }
}