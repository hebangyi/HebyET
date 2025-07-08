using Unity.Mathematics;

namespace ET
{
    [UnitEntityLogic]
    public class PlayerMoveTickLogic : IUnitEntityTickLogic
    {
        public void OnTick(World world)
        {
            var allPlayers = world.AllPlayers;
            foreach (var playerKv in allPlayers)
            {
                var unitEntityPosition = playerKv.Value.GetUnitEntityElemData<UnitEntityPosition>();
                var unitEntityPlayerOperationAction = playerKv.Value.GetUnitEntityElemData<UnitEntityPlayerOperationAction>();
                var unitEntityInfo = playerKv.Value.GetUnitEntityElemData<UnitEntityInfo>();
                
                // 记录脏数据
                unitEntityPosition.Position += unitEntityInfo.Speed * new float3(0, 0, 0.1f);
            }
        }
    }
}