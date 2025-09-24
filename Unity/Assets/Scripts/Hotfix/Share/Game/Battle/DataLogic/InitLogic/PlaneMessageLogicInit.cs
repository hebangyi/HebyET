namespace ET
{
    [UnitEntityLogic]
    public class PlaneMessageInit: IBattleInit
    {
        public void OnInit(UnitEntity unitEntity)
        {
            unitEntity.LogicWorld().UnitEntityMap = unitEntity;
        }

        public void OnDestroy(UnitEntity unitEntity)
        {
        }

        public ushort WatchComponentId()
        {
            return OpcodeType.Instance.GetOpcode(typeof(UnitEntityMapMessage));
        }
    }
}
