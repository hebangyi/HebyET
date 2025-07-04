using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class BattleUnitEntityDataLogicManagerComponent: Entity, IAwake
    {
        public static BattleUnitEntityDataLogicManagerComponent Instance;
        
        public Dictionary<int, List<IUnitEntityInitLogic>> CompId2InitLogics = new ();

        public Dictionary<int, List<IUnitEntityTickLogic>> CompId2TickLogics = new();
    }    
}