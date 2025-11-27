namespace ET
{
    [LogicUnitEntityContext(UETypeEnum.PlantMessage)]
    public class PlantMessageContext: ILogicUnitEntityContext
    {
        
        public void Init(UnitEntity unitEntity)
        {
            unitEntity.LogicWorld().UnitEntityMap = unitEntity;
        }

        public void Destroy(UnitEntity unitEntity)
        {
        }
    }
}