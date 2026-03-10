using UnityEngine;

namespace ET.Client
{
    [UnitEntityViewLogic]
    public class CommonDataEleInitLogic: IClientEleInit
    {
        public ushort WatchComponentId()
        {
            return OpcodeType.Instance.GetOpcode(typeof(UnitEntityCommonData));
        }

        public void OnInit(ClientUnitEntity unitEntity, object eleData)
        {
            var unitEntityElemData = unitEntity.GetUnitEntityElemData<UnitEntityCommonData>();
            var clientWorld = unitEntity.ClientWorld();

            var showTypeEnum = unitEntityElemData.UELayerTypeEnum;
            if (showTypeEnum == UELayerTypeEnum.Env)
            {
                clientWorld.EvnUnitEntities[unitEntity.Id] = unitEntity;
            }
        }

        public void OnDestroy(ClientUnitEntity unitEntity, object eleData)
        {
            var unitEntityElemData = unitEntity.GetUnitEntityElemData<UnitEntityCommonData>();
            var clientWorld = unitEntity.ClientWorld();
            var showTypeEnum = unitEntityElemData.UELayerTypeEnum;
            if (showTypeEnum == UELayerTypeEnum.Env)
            {
                clientWorld.EvnUnitEntities.Remove(unitEntity.Id);
            }
        }
    }
}
