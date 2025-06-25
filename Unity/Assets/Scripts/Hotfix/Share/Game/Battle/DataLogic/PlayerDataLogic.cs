using System;
using System.Collections.Generic;

namespace ET
{
    [UnitEntityLogic]
    public class PlayerDataLogic : IUnitEntityInitLogic
    {
        public void OnInit(UnitEntity unitEntity)
        {
            var playerInfo = unitEntity.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            unitEntity.World.AllPlayers[playerInfo.playerId] = unitEntity;
        }

        public void OnDestroy(UnitEntity unitEntity)
        {
            var playerInfo = unitEntity.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            unitEntity.World.AllEntity.Remove(playerInfo.playerId);
        }

        public ushort[] WatchComponentIds()
        {
            return new ushort
            [
                OpcodeType.Instance.GetOpcode(typeof(UnitEntityPlayerInfo))
            ];
        }
    }
}