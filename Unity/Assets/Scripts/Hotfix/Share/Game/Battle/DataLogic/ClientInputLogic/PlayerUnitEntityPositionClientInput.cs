using System.Linq;

namespace ET
{
    [UnitEntityLogic]
    public class PlayerUnitEntityPositionClientInput : BaseLogicClientInput<UnitEntityPosition>
    {
        public override bool CanInput(UnitEntity unitEntity, UnitEntityPosition elementData)
        {
            if (BuffHelper.IsRigidity(unitEntity))
            {
                return false;
            }
            
            return true;
        }

        public override void Updated(UnitEntity unitEntity)
        {
            var unitEntityPosition = unitEntity.GetUnitEntityElemData<UnitEntityPosition>();
            
            # if DEBUG
            var playerAOICell = unitEntity.GetUnitEntityElemData<GizmosPlayerAOICell>();
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
            # endif
            
            // 更新AOI
            // 设置动画状态
            unitEntity.ChangeAnimateStatus(AnimateStateEnum.Run);
        }
    }
}

