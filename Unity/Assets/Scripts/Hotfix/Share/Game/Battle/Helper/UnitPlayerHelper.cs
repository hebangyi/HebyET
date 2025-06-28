using System.Collections.Generic;

namespace ET
{
    public  static class UnitPlayerHelper
    {
        public static UnitEntity Create(World world, long playerId)
        {
            UnitEntity entity = world.CreateEntity();
            UnitEntityInfo unitEntityInfo = entity.GetOrCreateUnitEntityElemData<UnitEntityInfo>();
            unitEntityInfo.UnitEntityTypeEnum = UnitEntityTypeEnum.Player;
            unitEntityInfo.ConfigId = 0;
        
            var unitEntityPlayerInfo = entity.GetOrCreateUnitEntityElemData<UnitEntityPlayerInfo>();
            unitEntityPlayerInfo.PlayerId = playerId;
            return entity;
        }


        public static void Online(World world, long playerId)
        {
            var unitEntity = world.AllPlayers.GetValueOrDefault(playerId);
            var playerInfo = unitEntity?.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            if (playerInfo == null)
            {
                playerInfo.IsOnline = true;
                playerInfo.LastLoginTime = TimeInfo.Instance.NowSec();
            }
        }

        public static void Offline(World world, long playerId)
        {
            var unitEntity = world.AllPlayers.GetValueOrDefault(playerId);
            var playerInfo = unitEntity?.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            if (playerInfo == null)
            {
                playerInfo.IsOnline = true;
                playerInfo.LastLoginTime = TimeInfo.Instance.NowSec();
            }
        }
    }
}

