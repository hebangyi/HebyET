namespace ET
{
    public class UnitEntityLogicAttribute : BaseAttribute
    {
    }

    public interface IUnitEntityLogic
    {
    }

    public interface IUnitEntityInitLogic : IUnitEntityLogic
    {
        // 创建初始化Entity的时触发
        void OnInit(UnitEntity unitEntity);

        // 在销毁的时候触发
        void OnDestroy(UnitEntity unitEntity);
        
        // 监听的 ComponentId
        ushort[] WatchComponentIds();
    }

    // 数据处理接口 会修改UnitEntity中的数据
    public interface IUnitEntityTickLogic : IUnitEntityLogic
    {
        // 执行更新
        void OnTick(World world);
    }
}