/*using UnityEngine;

namespace ET.Client
{
    [UnitEntityViewLogic]
    public class GizmosDebugUpdatePositionLogic : IClientEleUpdate, IClientEleInit
    {
        public void OnUpdate(UnitEntity unitEntity, IUnitEntityElemData oldData, IUnitEntityElemData newData)
        {
            var clientWorld = unitEntity.ClientWorld();
            var players = clientWorld.PlayerUnitEntities.Values;

            var gizmosDebug = ET.GizmosDebug.Instance;
            if (gizmosDebug == null)
            {
                return;
            }
            gizmosDebug.Spheres.Clear();
            
            if (unitEntity.InsId != clientWorld.MainPlayerId)
            {
                return;
            }

            GizmosWireSphere wireSphere = new();
            var unitEntityPosition = clientWorld.MainPlayer.GetUnitEntityElemData<UnitEntityPosition>();
            wireSphere.center = new Vector3(unitEntityPosition.Position.x, unitEntityPosition.Position.y, 0f);
            wireSphere.radius = GameConstant.AOIWatchRadius;
            gizmosDebug.Spheres.Add(wireSphere);
        }

        public void OnInit(UnitEntity unitEntity)
        {
            var clientWorld = unitEntity.ClientWorld();
            var players = clientWorld.PlayerUnitEntities.Values;

            var gizmosDebug = ET.GizmosDebug.Instance;
            if (gizmosDebug == null)
            {
                return;
            }
            
            gizmosDebug.Spheres.Clear();

            if (clientWorld.MainPlayer == null)
            {
                return;
            }
            
            GizmosWireSphere wireSphere = new();
            var unitEntityPosition = clientWorld.MainPlayer.GetUnitEntityElemData<UnitEntityPosition>();
            wireSphere.center = new Vector3(unitEntityPosition.Position.x, unitEntityPosition.Position.y, 0f);
            wireSphere.radius = GameConstant.AOIWatchRadius;
            gizmosDebug.Spheres.Add(wireSphere);

        }

        public void OnDestroy(UnitEntity unitEntity)
        {
        }

        public ushort WatchComponentId()
        {
            return OpcodeType.Instance.GetOpcode(typeof(UnitEntityPosition));
        }
    }
}*/