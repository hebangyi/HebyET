using System;
using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class BattleUnitEntityLogicManagerComponent: Entity, IAwake
    {
        public static BattleUnitEntityLogicManagerComponent Instance;
        
        public Dictionary<int, List<IUnitEntityInitLogic>> CompId2InitLogics = new ();

        public Dictionary<Type, IUnitEntityTickLogic> Type2TickLogics = new ();
    }    
}