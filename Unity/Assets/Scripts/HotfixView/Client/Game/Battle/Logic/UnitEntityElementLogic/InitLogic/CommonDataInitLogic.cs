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

        public void OnInit(UnitEntity unitEntity, object eleData)
        {
            var unitEntityElemData = unitEntity.GetUnitEntityElemData<UnitEntityCommonData>();
            var clientWorld = unitEntity.ClientWorld();

            var showTypeEnum = unitEntityElemData.UELayerTypeEnum;
            if (showTypeEnum == UELayerTypeEnum.Env)
            {
                clientWorld.EvnUnitEntities[unitEntity.Id] = unitEntity;
            }
        }

        public void OnDestroy(UnitEntity unitEntity, object eleData)
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
