namespace ET
{
    public class UnitEntityLogicAttribute : BaseAttribute
    {
    }

    public interface IBattle
    {
    }

    // 元素初始化调用
    public interface ILogicEleInit : IBattle
    {
        // 创建初始化Entity的时触发
        void OnInit(UnitEntity unitEntity);

        // 在销毁的时候触发
        void OnDestroy(UnitEntity unitEntity);
        
        // 监听的 ComponentId
        ushort WatchComponentId();
    }

    // 数据处理接口 Tick 逻辑执行
    public interface ILogicTick : IBattle
    {
        // 执行更新
        void OnTick(LogicWorld logicWorld);
    }

    public interface ILogicClientInput : IBattle
    {
        // 是否能输入
        bool CanInput(UnitEntity unitEntity, object newElementData);

        // 更新成功
        bool Updated(UnitEntity unitEntity);
        
        // 监听的组件ID
        ushort WatchComponentId();
    }

    public abstract class BaseLogicClientInput<T> : ILogicClientInput where T : class, IUnitEntityElemData
    {
        public bool CanInput(UnitEntity unitEntity, object newElementData)
        {
            var t = newElementData as T;
            if (t == null)
            {
                return false;
            }

            return CanInput(t);
        }

        public abstract bool CanInput(T elementData);
        
        public abstract bool Updated(UnitEntity unitEntity);

        public ushort WatchComponentId()
        {
            return OpcodeType.Instance.GetOpcode(typeof(T));
        }
    }
}