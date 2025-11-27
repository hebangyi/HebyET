using UnityEngine;

namespace ET.Client
{
    [UnitEntityViewLogic]
    public class CommonDataEleInitLogic: IClientEleInit
    {
        public void OnInit(UnitEntity unitEntity)
        {
            var unitEntityElemData = unitEntity.GetUnitEntityElemData<UnitEntityCommonData>();
            var clientWorld = unitEntity.ClientWorld();

            var showTypeEnum = unitEntityElemData.UEShowTypeEnum;
            if (showTypeEnum == UEShowTypeEnum.ENV)
            {
                clientWorld.EvnUnitEntities[unitEntity.Id] = unitEntity;
                var unitEntityGameObjectComponent = unitEntity.GetComponent<UnitEntityGameObjectComponent>();
                var gameObject = unitEntityGameObjectComponent.GameObject;
                gameObject.transform.rotation = Quaternion.Euler(-GameConstant.GameOperaAngle, 0, 0);
            }
        }

        public void OnDestroy(UnitEntity unitEntity)
        {
            var unitEntityElemData = unitEntity.GetUnitEntityElemData<UnitEntityCommonData>();
            var clientWorld = unitEntity.ClientWorld();
            var showTypeEnum = unitEntityElemData.UEShowTypeEnum;
            if (showTypeEnum == UEShowTypeEnum.ENV)
            {
                clientWorld.EvnUnitEntities.Remove(unitEntity.Id);
            }
        }

        public ushort WatchComponentId()
        {
            return OpcodeType.Instance.GetOpcode(typeof(UnitEntityCommonData));
        }
    }
}
