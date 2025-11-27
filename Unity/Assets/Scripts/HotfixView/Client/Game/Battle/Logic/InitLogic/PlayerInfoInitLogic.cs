namespace ET.Client
{
    [UnitEntityViewLogic]
    public class PlayerInfoElemEleInitLogic : IClientElemEleInit
    {
        public void OnInit(UnitEntity unitEntity)
        {
            var playerInfo = unitEntity.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            // 初始化 player Cache中的数据
        }

        public void OnDestroy(UnitEntity unitEntity)
        {
            var playerInfo = unitEntity.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            unitEntity.ClientWorld().PlayerUnitEntities.Remove(playerInfo.PlayerId);
        }
        
        public ushort WatchComponentId()
        {
            return OpcodeType.Instance.GetOpcode(typeof(UnitEntityPlayerInfo));
        }
    }
}