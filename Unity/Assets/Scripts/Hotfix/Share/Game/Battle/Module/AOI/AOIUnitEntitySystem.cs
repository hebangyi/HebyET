using Unity.Mathematics;

namespace ET
{
    [FriendOf(typeof(UnitEntity))]
    [EntitySystemOf(typeof(AOIUnitEntity))]
    public static partial class AOIUnitEntitySystem
    {
        [EntitySystem]
        private static void Awake(this AOIUnitEntity self, float2 position, UETypeEnum UEType)
        {
            self.UETypeEnum = UEType;
            
            var unitEntity = self.GetParent<UnitEntity>();
            var logicWorld = unitEntity.LogicWorld();
            var aoiManagerComponent = logicWorld.GetComponent<AOIManagerComponent>();
            
            Log.Info($"AOIUnitEntity X: {(int)position.x}, Y: {(int)position.y} , Type = {unitEntity.GetUnitEntityElemData<UnitEntityCommonData>().UnitEntityType}");
            
            var cellId = AOIHelper.GetCellId(position);
            aoiManagerComponent.AwakeCellUnitEntity(self, cellId);
        }

        [EntitySystem]
        private static void Destroy(this AOIUnitEntity self)
        {
            var logicWorld = self.GetParent<UnitEntity>().LogicWorld();
            var aoiManagerComponent = logicWorld.GetComponent<AOIManagerComponent>();
            aoiManagerComponent.DestroyCellUnitEntity(self);
        }

    }
}
