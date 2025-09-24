namespace ET.Client
{
    [UnitEntityViewLogic]
    public class PlayerClient : IClientInit
    {
        public void OnInit(UnitEntity unitEntity)
        {
            var playerInfo = unitEntity.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            unitEntity.ClientWorld().PlayerUnitEntities[playerInfo.PlayerId] = unitEntity;
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