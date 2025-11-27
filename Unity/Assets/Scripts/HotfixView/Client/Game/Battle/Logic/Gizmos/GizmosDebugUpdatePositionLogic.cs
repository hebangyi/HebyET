using UnityEngine;

namespace ET.Client
{
    [UnitEntityViewLogic]
    public class GizmosDebugUpdatePositionLogic : IClientEleUpdate
    {
        public void OnUpdate(UnitEntity unitEntity, IUnitEntityElemData oldData, IUnitEntityElemData newData)
        {
            var clientWorld = unitEntity.ClientWorld();
            var players = clientWorld.PlayerUnitEntities.Values;

            var gizmosDebug = ET.GizmosDebug.Instance;
            gizmosDebug.Spheres.Clear();

            foreach (var player in players)
            {
                GizmosWireSphere wireSphere = new();
                var unitEntityPosition = player.GetUnitEntityElemData<UnitEntityPosition>();
                
                wireSphere.center = new Vector3(unitEntityPosition.Position.x, unitEntityPosition.Position.y, 0f);
                wireSphere.radius = GameConstant.AOIWatchRadius;
                gizmosDebug.Spheres.Add(wireSphere);
            }
            
        }

        public void OnDestroy(UnitEntity unitEntity)
        {
        }

        public ushort WatchComponentId()
        {
            return OpcodeType.Instance.GetOpcode(typeof(UnitEntityPosition));
        }
    }
}