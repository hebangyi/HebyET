using System;

namespace ET
{
    [UnitEntityDataLogic]
    public class PlayerDataLogic : IUnitEntityDataLogic
    {
        public void OnInit(UnitEntity unitEntity)
        {
            
        }

        public void OnDestroy(UnitEntity unitEntity)
        {
            
        }

        public ushort[] WatchComponentIds()
        {
            return new ushort []
            {
                OpcodeType.Instance.GetOpcode(typeof(UnitEntityPlayerInfo))
            };
        }

        public void OnExecute(UnitEntity unitEntity)
        {
            
        }
    }
}