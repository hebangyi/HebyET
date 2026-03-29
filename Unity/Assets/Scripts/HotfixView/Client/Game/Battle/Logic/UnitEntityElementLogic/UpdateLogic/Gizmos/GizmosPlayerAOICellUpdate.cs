using UnityEngine;

namespace ET.Client
{
    [UnitEntityViewLogic]
    public class GizmosPlayerAOICellUpdate: BaseClientEleLogic<GizmosPlayerAOICell>
    {
        public override void OnInitT(ClientUnitEntity unitEntity, GizmosPlayerAOICell elemData)
        {
            SetGizmos(unitEntity, elemData);
        }

        public override void OnDestroyT(ClientUnitEntity unitEntity, GizmosPlayerAOICell elemData)
        {
        }

        public override void OnUpdateT(ClientUnitEntity unitEntity, GizmosPlayerAOICell oldData, GizmosPlayerAOICell newData)
        {
            SetGizmos(unitEntity, newData);
        }
        
        
        public void SetGizmos(ClientUnitEntity unitEntity, GizmosPlayerAOICell gizmosPlayerAOICell)
        {
            var instance = GizmosDebug.Instance;
            if (instance == null)
            {
                return;
            }
            
            instance.AOICells.Clear();
            foreach (var cellId in gizmosPlayerAOICell.CellIds)
            {
                var (x,y) = AOIHelper.GetCellXY(cellId);
                var pointX = x * GameConstant.AOICellSize + GameConstant.AOICellSize / 2;
                var pointY = y * GameConstant.AOICellSize + GameConstant.AOICellSize / 2;
                
                instance.AOICells.Add(new Vector3(pointX, pointY));
            }
        }
        
    }
}

