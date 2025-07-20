namespace ET
{
    [UnitEntityLogic]
    public class PlantInitLogic : IUnitEntityInitLogic
    {
        public void OnInit(UnitEntity unitEntity)
        {
            var world = unitEntity.World;
            var plantCellInfo = unitEntity.GetUnitEntityElemData<PlantCellInfo>();
            var center = plantCellInfo.Center;

            world.AllPlants.Add(unitEntity);
            world.Point2Plants[center] = unitEntity;
        }

        public void OnDestroy(UnitEntity unitEntity)
        {
        }

        public ushort WatchComponentId()
        {
            return OpcodeType.Instance.GetOpcode(typeof(PlantCellInfo));
        }
    }    
}
