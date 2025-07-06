using System.Collections.Generic;

namespace ET.Client
{
    public static class ClientWorldHelper
    {
        public static void InitWorld(this World world, BattleWorld battleWorld, List<BattleUnitEntity> battleUnitEntities)
        {
            world.WorldStatusEnum = battleWorld.WorldStatus;
            world.Frame = battleWorld.Frame;

            foreach (var battleUnitEntity in battleUnitEntities)
            {
                world.CreateEntity(battleUnitEntity);
            }
        }
    }
}