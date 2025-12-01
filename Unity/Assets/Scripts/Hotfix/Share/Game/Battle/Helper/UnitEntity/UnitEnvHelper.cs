using Unity.Mathematics;

namespace ET
{
    public static class UnitEnvHelper
    {
        public static UnitEntity Create(LogicWorld logicWorld, float2 position)
        {
            var unitEntity = logicWorld.CreateEntity();
            var unitEntityCommonData = unitEntity.CreateUnitEntityElemData<UnitEntityCommonData>();
            unitEntityCommonData.UnitEntityType = UETypeEnum.tree_1;
            unitEntityCommonData.UELayerTypeEnum = UELayerTypeEnum.Env;
            
            var unitEntityPosition = unitEntity.CreateUnitEntityElemData<UnitEntityPosition>();
            unitEntityPosition.Position = position;
            
            unitEntity.AddComponent<AOIUnitEntity, float2, UETypeEnum>(unitEntityPosition.Position, UETypeEnum.tree_1);
            return unitEntity;
        }
    }
    
}