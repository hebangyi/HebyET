namespace ET
{
    [LogicUnitEntityContext(UETypeEnum.GizmosDebug)]
    public class GizmosDebugLogicContext: ILogicUnitEntityContext
    {
        public void Init(UnitEntity unitEntity)
        {
            unitEntity.LogicWorld().GizmosDebugUnitEntity = unitEntity;
        }

        public void Destroy(UnitEntity unitEntity)
        {
        }
    }
}