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
        void OnInit(ClientUnitEntity unitEntity, object eleData);

        // 在销毁的时候触发
        void OnDestroy(ClientUnitEntity unitEntity, object eleData);
    }


    public interface IClientEleUpdate : IClient
    {
        public void OnUpdate(ClientUnitEntity unitEntity, IUnitEntityElemData oldData, IUnitEntityElemData newData);
    }

    public abstract class BaseClientEleLogic<T> : IClientEleInit, IClientEleUpdate where T : class
    {
        public ushort WatchComponentId()
        {
            return OpcodeType.Instance.GetOpcode(typeof(T));
        }

        public void OnInit(ClientUnitEntity unitEntity, object eleData)
        {
            var elemData = eleData as T;
            OnInitT(unitEntity, elemData);
        }

        public void OnDestroy(ClientUnitEntity unitEntity, object eleData)
        {
            var elemData = eleData as T;
            OnDestroyT(unitEntity, elemData);
        }
        
        public abstract void OnInitT(ClientUnitEntity unitEntity, T elemData);

        public abstract void OnDestroyT(ClientUnitEntity unitEntity, T elemData);

        public void OnUpdate(ClientUnitEntity unitEntity, IUnitEntityElemData oldData, IUnitEntityElemData newData)
        {
            var oldT = oldData as T;
            var newT = newData as T;
            this.OnUpdateT(unitEntity, oldT, newT);
        }

        public abstract void OnUpdateT(ClientUnitEntity unitEntity, T oldData, T newData);

    }
    
    
    
    public interface IClientUnitEntityContext
    {
        void Init(ClientUnitEntity unitEntity);
        void CreateView(ClientUnitEntity unitEntity);
        void InitView(ClientUnitEntity unitEntity);
        void Destroy(ClientUnitEntity unitEntity);
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
