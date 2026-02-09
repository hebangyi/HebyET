namespace ET
{
    public class UnitEntityLogicAttribute : BaseAttribute
    {
    }

    public interface IBattleElem
    {
        // 监听的 ComponentId
        ushort WatchComponentId(); 
    }

    // 元素初始化调用
    public interface ILogicEleInit : IBattleElem
    {
        // 创建初始化Entity的时触发
        void OnInit(UnitEntity unitEntity);

        // 在销毁的时候触发
        void OnDestroy(UnitEntity unitEntity);
    }

    // 数据处理接口 Tick 逻辑执行
    public interface ILogicTickUpdate
    {
        // 执行更新
        void OnTick(LogicWorld logicWorld);
    }

    public interface ILogicClientInput : IBattleElem
    {
        // 是否能输入
        bool CanInput(UnitEntity unitEntity, object newElementData);

        // 更新成功
        void Updated(UnitEntity unitEntity);
    }
    
    public class LogicUnitEntityContext : BaseAttribute
    {
        public UELayerTypeEnum LayerType;
        
        public UETypeEnum UeTypeEnum;

        public LogicUnitEntityContext(UELayerTypeEnum UeLayerType, UETypeEnum UeTypeEnum)
        {
            this.LayerType = UeLayerType;
            this.UeTypeEnum = UeTypeEnum;
        }
    }
    
    public interface ILogicUnitEntityContext
    {
        // 初始化常规数据
        void InitCommonData(UnitEntity unitEntity);
        
        // 初始化自定义数据
        void InitCustomData(UnitEntity unitEntity);
        
        // 初始化数值
        void InitNumericalData(UnitEntity unitEntity);
        // 初始化组件
        void InitComponent(UnitEntity unitEntity);
        void Destroy(UnitEntity unitEntity);
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
        
        public abstract void Updated(UnitEntity unitEntity);

        public ushort WatchComponentId()
        {
            return OpcodeType.Instance.GetOpcode(typeof(T));
        }
    }
}