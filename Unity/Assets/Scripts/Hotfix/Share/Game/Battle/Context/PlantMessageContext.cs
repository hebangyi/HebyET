namespace ET
{
    [LogicUnitEntityContext(UETypeEnum.PlantMessage)]
    public class PlantMessageContext: ILogicUnitEntityContext
    {
        
        public void Init(UnitEntity unitEntity)
        {
            unitEntity.LogicWorld().PlantMessageUnitEntity = unitEntity;
        }

        public void Destroy(UnitEntity unitEntity)
        {
        }
    }
}