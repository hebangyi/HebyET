using System.Linq;
using Unity.Mathematics;

namespace ET
{
    [LogicUnitEntityContext(UELayerTypeEnum.Player, UETypeEnum.Player)]
    public class PlayerLogicContext : BaseLogicUnitEntityContext
    {
        public override void InitCustomData(UnitEntity unitEntity)
        {
            var unitEntityInitContext = unitEntity.GetComponent<UnitEntityInitContext>();
            unitEntity.CreateUnitEntityElemData<GizmosPlayerAOICell>();
            unitEntity.CreateUnitEntityElemData<UnitEntityCameraData>();
            unitEntity.CreateUnitEntityElemData<UnitEntityTowardAngle>();
            unitEntity.CreateUnitEntityElemData<UnitEntityPlayerAnimateStatus>();
            var unitEntityPosition = unitEntity.CreateUnitEntityElemData<UnitEntityPosition>();
            unitEntityPosition.Position = new float2(0f, 0f);
            
            UnitEntityInfo unitEntityInfo = unitEntity.CreateUnitEntityElemData<UnitEntityInfo>();
            unitEntityInfo.ConfigId = 0;
            unitEntityInfo.Speed = 30;
            
            var unitEntityPlayerInfo = unitEntity.CreateUnitEntityElemData<UnitEntityPlayerInfo>();
            var playerId = unitEntityInitContext.Params as long?;
            unitEntityPlayerInfo.PlayerId = playerId.GetValueOrDefault();
        }
        

        public override  void Init(UnitEntity unitEntity)
        {
            var playerInfo = unitEntity.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            var gizmosPlayerAOICell = unitEntity.GetUnitEntityElemData<GizmosPlayerAOICell>();
            var unitEntityPosition = unitEntity.GetUnitEntityElemData<UnitEntityPosition>();
            unitEntity.LogicWorld().PlayerId2Players[playerInfo.PlayerId] = unitEntity;
            
            // 随机选择一个地块
            var logicWorld = unitEntity.LogicWorld();
            var unitEntityMap = logicWorld.PlantMessageUnitEntity;
            
            var unitEntityMapMessage = unitEntityMap.GetUnitEntityElemData<UnitEntityMapMessage>();
            var cellInfo = unitEntityMapMessage.PlantInfo.CellInfos.FirstOrDefault();
            if (cellInfo != null)
            {
                unitEntityPosition.Position = cellInfo.CenterPoint;
                unitEntityPosition.Position += new float2(10, 10);
            }

            unitEntity.AddComponent<PlayerAOISeeUnitEntity>();
            unitEntity.AddComponent<AOIUnitEntity, float2, UETypeEnum>(unitEntityPosition.Position, UETypeEnum.Player);
            
            
            var cellIds = AOIHelper.GetAOICellIds(unitEntityPosition.Position);
            gizmosPlayerAOICell.CellIds.AddRange(cellIds);
        }

        public override  void Destroy(UnitEntity unitEntity)
        {
            var playerInfo = unitEntity.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            unitEntity.LogicWorld().PlayerId2Players.Remove(playerInfo.PlayerId);
        }
    }
}