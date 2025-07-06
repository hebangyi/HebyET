namespace ET.Client
{
    [UnitEntityViewLogic]
    public class PlayerViewLogic : IUnitEntityViewInitLogic
    {
        public ushort[] WatchComponentIds()
        {
            return new[] { OpcodeType.Instance.GetOpcode(typeof(UnitEntityPlayerInfo)) };
        }

        public void OnInit(UnitEntity unitEntity)
        {
            var playerInfo = unitEntity.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            unitEntity.World.ViewAllPlayers[playerInfo.PlayerId] = unitEntity;
        }

        public void OnDestroy(UnitEntity unitEntity)
        {
            var playerInfo = unitEntity.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            unitEntity.World.ViewAllPlayers.Remove(playerInfo.PlayerId);
        }
    }
}