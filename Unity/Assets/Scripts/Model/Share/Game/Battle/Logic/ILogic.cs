namespace ET
{
    public class UnitEntityLogicAttribute : BaseAttribute
    {
    }

    public interface IBattleLogic
    {
    }

    public interface IBattleLogicInit : IBattleLogic
    {
        // 创建初始化Entity的时触发
        void OnInit(UnitEntity unitEntity);

        // 在销毁的时候触发
        void OnDestroy(UnitEntity unitEntity);
        
        // 监听的 ComponentId
        ushort WatchComponentId();
    }

    // 数据处理接口 会修改UnitEntity中的数据
    public interface IBattleLogicTick : IBattleLogic
    {
        // 执行更新
        void OnTick(LogicWorld logicWorld);
    }
}