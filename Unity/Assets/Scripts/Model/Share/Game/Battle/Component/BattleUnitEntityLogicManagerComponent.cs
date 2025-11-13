using System;
using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class BattleUnitEntityLogicManagerComponent: Entity, IAwake
    {
        public static BattleUnitEntityLogicManagerComponent Instance;
        
        public Dictionary<int, List<ILogicEleInit>> CompId2InitLogics = new ();

        public Dictionary<Type, ILogicTick> Type2TickLogics = new ();
    }    
}