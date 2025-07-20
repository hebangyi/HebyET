using System;
using System.Collections.Generic;

namespace ET
{
    [UnitEntityLogic]
    public class PlayerDataInitLogic : IUnitEntityInitLogic
    {
        public void OnInit(UnitEntity unitEntity)
        {
            var playerInfo = unitEntity.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            unitEntity.World.PlayerId2Players[playerInfo.PlayerId] = unitEntity;
        }

        public void OnDestroy(UnitEntity unitEntity)
        {
            var playerInfo = unitEntity.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            unitEntity.World.PlayerId2Players.Remove(playerInfo.PlayerId);
        }

        public ushort WatchComponentId()
        {
            return OpcodeType.Instance.GetOpcode(typeof(UnitEntityPlayerInfo));
        }
    }
}