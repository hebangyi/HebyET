using System;
using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class BattleUnitEntityLogicManagerComponent: Entity, IAwake
    {
        public static BattleUnitEntityLogicManagerComponent Instance;
        
        public Dictionary<int, List<IBattleInit>> CompId2InitLogics = new ();

        public Dictionary<Type, IBattleTick> Type2TickLogics = new ();
    }    
}