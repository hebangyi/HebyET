using Unity.Mathematics;

namespace ET
{
    public static class UnitMonsterHelper
    {
        public static UnitEntity Create(LogicWorld logicWorld, float2 position)
        {
            return logicWorld.Create(UETypeEnum.Monster, position);
        }
    }
}
