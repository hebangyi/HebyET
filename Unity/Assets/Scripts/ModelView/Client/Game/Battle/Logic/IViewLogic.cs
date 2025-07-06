namespace ET.Client
{
    public class UnitEntityViewLogicAttribute : BaseAttribute
    {
    }
    
    public interface IViewLogic
    {
        ushort[] WatchComponentIds();
    }
    
    public interface IUnitEntityViewInitLogic : IViewLogic
    {
        // 创建初始化Entity的时触发
        void OnInit(UnitEntity unitEntity);

        // 在销毁的时候触发
        void OnDestroy(UnitEntity unitEntity);
    }


    public interface IUnitEntityViewElementDataUpdateLogic : IViewLogic
    {
        public void OnUpdate(UnitEntity unitEntity, IUnitEntityElemData oldData, IUnitEntityElemData newData);
    }
}

