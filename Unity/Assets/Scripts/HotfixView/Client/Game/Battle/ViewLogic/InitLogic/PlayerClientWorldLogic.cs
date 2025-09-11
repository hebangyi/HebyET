namespace ET.Client
{
    [UnitEntityViewLogic]
    public class PlayerClientWorldLogic : IUnitEntityClientWorldInitLogic
    {
        public ushort WatchComponentId()
        {
            return OpcodeType.Instance.GetOpcode(typeof(UnitEntityPlayerInfo));
        }

        public void OnInit(UnitEntity unitEntity)
        {
            var playerInfo = unitEntity.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            unitEntity.ClientWorld.ViewAllPlayers[playerInfo.PlayerId] = unitEntity;
        }

        public void OnDestroy(UnitEntity unitEntity)
        {
            var playerInfo = unitEntity.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            unitEntity.ClientWorld.ViewAllPlayers.Remove(playerInfo.PlayerId);
        }
    }
}