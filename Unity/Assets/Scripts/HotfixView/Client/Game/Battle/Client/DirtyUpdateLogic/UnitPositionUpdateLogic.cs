using UnityEngine;

namespace ET.Client
{
    [UnitEntityViewLogic]
    public class UnitPositionUpdate : IClientEleUpdate
    {
        public ushort WatchComponentId()
        {
            return OpcodeType.Instance.GetOpcode(typeof(UnitEntityPosition));
        }

        public void OnUpdate(UnitEntity unitEntity, IUnitEntityElemData oldData, IUnitEntityElemData newData)
        {
            var unitEntityPosition = newData as UnitEntityPosition;
            if (unitEntityPosition == null)
            {
                return;
            }
            
            var unitEntityGameObjectComponent = unitEntity.GetComponent<UnitEntityGameObjectComponent>();
            if (unitEntityGameObjectComponent != null && unitEntityGameObjectComponent.GameObject)
            {
                unitEntityGameObjectComponent.GameObject.transform.position = new Vector3(unitEntityPosition.Position.x, 0 , unitEntityPosition.Position.y);
            }
        }
    }    
}

