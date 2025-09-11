using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class BattleUnitEntityViewLogicManagerComponent: Entity, IAwake
    {
        public static BattleUnitEntityViewLogicManagerComponent Instance;
        
        public Dictionary<int, List<IUnitEntityClientWorldInitLogic>> CompId2InitViewLogics = new ();
        
        public Dictionary<int, List<IUnitEntityClientWorldElementDataUpdateLogic>> CompId2ElementDataUpdates = new ();
    }
}

