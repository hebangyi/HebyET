namespace ET
{
    public class UnitEntityDataLogicAttribute : BaseAttribute
    {
        
    }

    public interface IUnitEntityLogic
    {
        // 创建初始化Entity的时触发
        void OnInit(UnitEntity unitEntity);

        // 在销毁的时候触发
        void OnDestroy(UnitEntity unitEntity);

        // 监听的 ComponentId
        int[] WatchComponentIds();
    }

    // 数据处理接口 会修改UnitEntity中的数据
    public interface IUnitEntityDataLogic : IUnitEntityLogic
    {
        // 执行
        void OnExecute(UnitEntity unitEntity);
    }

    // 显示接口
    public interface IUnitEntityClientLogic : IUnitEntityLogic
    {
        // 执行
        void OnUpdateEntity(UnitEntity unitEntity, IUnitEntityElemData oldData);
    }
}