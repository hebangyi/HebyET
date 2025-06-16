namespace ET.Battle.Factory;

public static class UnitPlayerFactory
{
    public static UnitEntity Create(World world, long playerId)
    {
        UnitEntity entity = world.CreateEntity();
        UnitEntityInfo unitEntityInfo = entity.GetOrCreateUnitEntityElemData<UnitEntityInfo>();
        unitEntityInfo.unitEntityTypeEnum = UnitEntityTypeEnum.Player;
        unitEntityInfo.ConfigId = 0;
        
        var unitEntityPlayerInfo = entity.GetOrCreateUnitEntityElemData<UnitEntityPlayerInfo>();
        unitEntityPlayerInfo.playerId = playerId;
        return entity;
    }
}