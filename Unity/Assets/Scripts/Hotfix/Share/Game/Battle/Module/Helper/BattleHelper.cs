using System;
using System.Numerics;
using Unity.Mathematics;

namespace ET
{
    public static class BattleHelper
    {
        public static float Distance(float2 from, float2 to)
        {
            return (float)Math.Sqrt((double)(from.x - to.x)*(from.x - to.x) + (double)(from.y - to.y)*(from.y - to.y));
        }

        public static void ChangePosition(UnitEntity unitEntity, float2 position)
        {
            var unitEntityPosition = unitEntity.GetUnitEntityElemData<UnitEntityPosition>();
            if (unitEntityPosition == null)
            {
                return;
            }

            unitEntityPosition.Position = position;
            var newCellId = AOIHelper.GetCellId(unitEntityPosition.Position);
            var aoiUnitEntity = unitEntity.GetComponent<AOIUnitEntity>();
            if (aoiUnitEntity != null && newCellId != aoiUnitEntity.CellId)
            {
                var logicWorld = unitEntity.LogicWorld();
                var aoiManagerComponent = logicWorld.GetComponent<AOIManagerComponent>();
                aoiManagerComponent.MoveCell(aoiUnitEntity, newCellId);
            }
        }
    }
}

