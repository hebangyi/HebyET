using UnityEngine;

namespace ET.Client
{
    [UnitEntityViewLogic]
    public class GizmosPlayerAOICellUpdate: IClientEleUpdate, IClientEleInit
    {
        public ushort WatchComponentId()
        {
            return OpcodeType.Instance.GetOpcode(typeof(GizmosPlayerAOICell));
        }

        public void OnUpdate(UnitEntity unitEntity, IUnitEntityElemData oldData, IUnitEntityElemData newData)
        {
            SetGizmos(unitEntity);
        }

        public void OnInit(UnitEntity unitEntity)
        {
            SetGizmos(unitEntity);
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
        
        public void OnDestroy(UnitEntity unitEntity)
        {
        }
    }
}

