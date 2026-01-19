using UnityEngine;

namespace ET.Client
{
    [UnitEntityViewLogic]
    public class GizmosPlayerAOICellUpdate: BaseClientEleLogic<GizmosPlayerAOICell>
    {
        public override void OnUpdateT(UnitEntity unitEntity, GizmosPlayerAOICell oldData, GizmosPlayerAOICell newData)
        {
            SetGizmos(unitEntity);
        }

        public override void OnInit(UnitEntity unitEntity)
        {
            SetGizmos(unitEntity);
        }
        
        public override void OnDestroy(UnitEntity unitEntity)
        {
        }
        
        public void SetGizmos(UnitEntity unitEntity)
        {
            var gizmosPlayerAOICell = unitEntity.GetUnitEntityElemData<GizmosPlayerAOICell>();
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

