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

            var showTypeEnum = unitEntityElemData.UELayerTypeEnum;
            if (showTypeEnum == UELayerTypeEnum.Env)
            {
                clientWorld.EvnUnitEntities[unitEntity.Id] = unitEntity;
            }
        }

        public void OnDestroy(UnitEntity unitEntity)
        {
            var unitEntityElemData = unitEntity.GetUnitEntityElemData<UnitEntityCommonData>();
            var clientWorld = unitEntity.ClientWorld();
            var showTypeEnum = unitEntityElemData.UELayerTypeEnum;
            if (showTypeEnum == UELayerTypeEnum.Env)
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
