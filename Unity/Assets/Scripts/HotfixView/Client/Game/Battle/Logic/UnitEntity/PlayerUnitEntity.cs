namespace ET.Client
{
    [ClientLifeCycle(UeTypeEnum = UETypeEnum.Player)]
    public class PlayerUnitEntityContext : BaseUnitEntityContext
    {
        public override void Init(UnitEntity unitEntity)
        {
            unitEntity.AddComponent<PlayerCacheDataComponent>();

            var clientWorld = unitEntity.ClientWorld();
            var playerInfo = unitEntity.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            clientWorld.PlayerUnitEntities[playerInfo.PlayerId] = unitEntity;
        }

        public override void Destroy(UnitEntity unitEntity)
        {
            var playerInfo = unitEntity.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            var clientWorld = unitEntity.ClientWorld();
            clientWorld.PlayerUnitEntities.Remove(playerInfo.PlayerId);
        }
    }
}
