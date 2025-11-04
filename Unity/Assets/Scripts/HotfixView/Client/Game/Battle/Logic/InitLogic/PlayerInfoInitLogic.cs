namespace ET.Client
{
    [UnitEntityViewLogic]
    public class PlayerInfoEleInitLogic : IClientEleInit
    {
        public void OnInit(UnitEntity unitEntity)
        {
            var playerInfo = unitEntity.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            
            
            unitEntity.ClientWorld().PlayerUnitEntities[playerInfo.PlayerId] = unitEntity;
            var playerCacheInfoComponent = unitEntity.AddComponent<PlayerCacheInfoComponent>();
            
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