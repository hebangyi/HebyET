using Unity.Mathematics;

namespace ET
{
    public static class UnitEnvHelper
    {
        public static UnitEntity Create(LogicWorld logicWorld, float2 position)
        {
            return logicWorld.Create(UETypeEnum.tree_1, position);
        }
    }
    
}