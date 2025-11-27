using System;
using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class LogicWorldLogicManagerComponent: Entity, IAwake
    {
        public static LogicWorldLogicManagerComponent Instance;
        
        public Dictionary<int, List<ILogicEleInit>> CompId2InitLogics = new ();

        public Dictionary<Type, ILogicTickUpdate> Type2TickLogics = new ();

        public Dictionary<int, ILogicClientInput> ClientInputLogics = new();
        
        public Dictionary<UETypeEnum, ILogicUnitEntityContext> UnitEntityContexts = new ();
    }    
}