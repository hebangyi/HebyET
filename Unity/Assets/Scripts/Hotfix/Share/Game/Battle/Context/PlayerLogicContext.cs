using System.Linq;
using Unity.Mathematics;

namespace ET
{
    [LogicUnitEntityContext(UETypeEnum.Player)]
    public class PlayerLogicContext : ILogicUnitEntityContext
    {
        public void InitElementData(UnitEntity unitEntity)
        {
            var playerInitInfo = unitEntity.GetComponent<PlayerInitContext>();
            var unitEntityCommonData = unitEntity.CreateUnitEntityElemData<UnitEntityCommonData>();
            unitEntityCommonData.UnitEntityType = UETypeEnum.Player;
            unitEntityCommonData.UELayerTypeEnum = UELayerTypeEnum.Player;
            
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
            unitEntityPlayerInfo.PlayerId = playerInitInfo.PlayerId;
        }

        public void InitLogicElementData(UnitEntity unitEntity)
        {
        }

        public void Init(UnitEntity unitEntity)
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

        public void Destroy(UnitEntity unitEntity)
        {
            var playerInfo = unitEntity.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            unitEntity.LogicWorld().PlayerId2Players.Remove(playerInfo.PlayerId);
        }
    }
}

