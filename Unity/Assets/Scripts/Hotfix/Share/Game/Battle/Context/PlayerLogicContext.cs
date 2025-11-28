using System.Linq;
using Unity.Mathematics;

namespace ET
{
    [LogicUnitEntityContext(UETypeEnum.Player)]
    public class PlayerLogicContext : ILogicUnitEntityContext
    {
        public void Init(UnitEntity unitEntity)
        {
            var playerInfo = unitEntity.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            unitEntity.LogicWorld().PlayerId2Players[playerInfo.PlayerId] = unitEntity;
            
            // 随机选择一个地块
            var logicWorld = unitEntity.LogicWorld();
            var unitEntityMap = logicWorld.PlantMessageUnitEntity;
            
            var unitEntityMapMessage = unitEntityMap.GetUnitEntityElemData<UnitEntityMapMessage>();
            var cellInfo = unitEntityMapMessage.PlantInfo.CellInfos.FirstOrDefault();
            if (cellInfo != null)
            {
                var unitEntityPosition = unitEntity.GetUnitEntityElemData<UnitEntityPosition>();
                unitEntityPosition.Position = cellInfo.CenterPoint;
                unitEntityPosition.Position += new float2(10, 10);
                
                unitEntity.AddComponent<AOIUnitEntity, float2>(unitEntityPosition.Position);
            }
        }

        public void Destroy(UnitEntity unitEntity)
        {
            var playerInfo = unitEntity.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            unitEntity.LogicWorld().PlayerId2Players.Remove(playerInfo.PlayerId);
        }
    }
}

