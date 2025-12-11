using Unity.Mathematics;

namespace ET
{
    public static class UnitTreeHelper
    {
        public static UnitEntity Create(LogicWorld logicWorld, float2 position)
        {
            return logicWorld.Create(UETypeEnum.Tree, position);
        }
    }
    
}