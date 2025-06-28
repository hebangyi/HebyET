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
            unitEntity.World.AllPlayers[playerInfo.PlayerId] = unitEntity;
        }

        public void OnDestroy(UnitEntity unitEntity)
        {
            var playerInfo = unitEntity.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            unitEntity.World.AllPlayers.Remove(playerInfo.PlayerId);
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