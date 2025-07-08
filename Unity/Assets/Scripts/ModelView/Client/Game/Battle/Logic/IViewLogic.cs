namespace ET.Client
{
    public class UnitEntityViewLogicAttribute : BaseAttribute
    {
    }
    
    public interface IViewLogic
    {
        
    }
    
    public interface IUnitEntityViewInitLogic : IViewLogic
    {
        // 创建初始化Entity的时触发
        void OnInit(UnitEntity unitEntity);

        // 在销毁的时候触发
        void OnDestroy(UnitEntity unitEntity);
        
        // 监听的 ComponentId
        ushort WatchComponentId();
    }


    public interface IUnitEntityViewElementDataUpdateLogic : IViewLogic
    {
        ushort WatchComponentId();
        public void OnUpdate(UnitEntity unitEntity, IUnitEntityElemData oldData, IUnitEntityElemData newData);
    }
    
}
