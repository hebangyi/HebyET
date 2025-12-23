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
