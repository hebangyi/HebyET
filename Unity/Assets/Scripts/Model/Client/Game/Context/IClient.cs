namespace ET.Client
{
    public class UnitEntityViewLogicAttribute : BaseAttribute
    {
    }
    
    public interface IClient
    {
        // 监听的 ComponentId
        ushort WatchComponentId();
    }
    
    // 元素初始化调用
    public interface IClientEleInit : IClient
    {
        // 创建初始化Entity的时触发
        void OnInit(UnitEntity unitEntity);

        // 在销毁的时候触发
        void OnDestroy(UnitEntity unitEntity);
    }


    public interface IClientEleUpdate : IClient
    {
        public void OnUpdate(UnitEntity unitEntity, IUnitEntityElemData oldData, IUnitEntityElemData newData);
    }

    public abstract class BaseClientEleLogic<T> : IClientEleInit, IClientEleUpdate where T : class
    {
        public ushort WatchComponentId()
        {
            return OpcodeType.Instance.GetOpcode(typeof(T));
        }
        
        public abstract void OnInit(UnitEntity unitEntity);

        public abstract void OnDestroy(UnitEntity unitEntity);

        public void OnUpdate(UnitEntity unitEntity, IUnitEntityElemData oldData, IUnitEntityElemData newData)
        {
            var oldT = oldData as T;
            var newT = newData as T;
            this.OnUpdateT(unitEntity, oldT, newT);
        }

        public abstract void OnUpdateT(UnitEntity unitEntity, T oldData, T newData);
    }
    
    
    
    public interface IClientUnitEntityContext
    {
        void Init(UnitEntity unitEntity);
        void CreateView(UnitEntity unitEntity);
        void InitView(UnitEntity unitEntity);
        void Destroy(UnitEntity unitEntity);
    }

    public class ClientUnitEntityContext : BaseAttribute
    {
        public UETypeEnum UeTypeEnum;

        public ClientUnitEntityContext(UETypeEnum UeTypeEnum)
        {
            this.UeTypeEnum = UeTypeEnum;
        }
    }
}
