namespace ET
{
    public abstract class BaseLogicUnitEntityContext : ILogicUnitEntityContext
    {
        public virtual void InitElementData(UnitEntity unitEntity)
        {
        }

        public virtual void InitLogicElementData(UnitEntity unitEntity)
        {
        }

        public virtual void Init(UnitEntity unitEntity)
        {
        }

        public virtual void Destroy(UnitEntity unitEntity)
        {
        }
    }
    
    [LogicUnitEntityContext(UETypeEnum.None)]
    public class DefaultLogicEntityContext : BaseLogicUnitEntityContext
    {
        
    }
}

