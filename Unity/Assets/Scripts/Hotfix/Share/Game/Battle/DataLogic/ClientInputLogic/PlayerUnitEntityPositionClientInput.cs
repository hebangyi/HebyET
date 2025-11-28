using System.Linq;

namespace ET
{
    [UnitEntityLogic]
    public class PlayerUnitEntityPositionClientInput : BaseLogicClientInput<UnitEntityPosition>
    {
        public override bool CanInput(UnitEntityPosition elementData)
        {
            return true;
        }

        public override void Updated(UnitEntity unitEntity)
        {
            var playerAOICell = unitEntity.GetUnitEntityElemData<GizmosPlayerAOICell>();
            var unitEntityPosition = unitEntity.GetUnitEntityElemData<UnitEntityPosition>();

            var cellIds = playerAOICell.CellIds;
            var newCellIds = AOIHelper.GetAOICellIds(unitEntityPosition.Position);

            bool isChange = false;
            if (cellIds.Count != newCellIds.Length)
            {
                isChange = true;
            }
            else
            {
                for (int i = 0; i < cellIds.Count; i++)
                {
                    if (cellIds[i] != newCellIds[i])
                    {
                        isChange = true;
                    }
                }
            }

            if (isChange)
            {
                playerAOICell.CellIds = newCellIds.ToList();
            }
        }
    }
}

