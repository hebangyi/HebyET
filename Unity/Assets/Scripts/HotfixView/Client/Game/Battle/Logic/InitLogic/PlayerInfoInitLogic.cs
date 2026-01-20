namespace ET.Client
{
    [UnitEntityViewLogic]
    public class PlayerInfoEleInitLogic : IClientEleInit
    {
        
        public ushort WatchComponentId()
        {
            return OpcodeType.Instance.GetOpcode(typeof(UnitEntityPlayerInfo));
        }

        public void OnInit(UnitEntity unitEntity, object eleData)
        {
            var playerInfo = unitEntity.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            // 初始化 player Cache中的数据
        }

        public void OnDestroy(UnitEntity unitEntity, object eleData)
        {
            var playerInfo = unitEntity.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            unitEntity.ClientWorld().PlayerUnitEntities.Remove(playerInfo.PlayerId);
        }
    }
}