using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class UnitEntityDataLogicManagerComponent: Entity, IAwake
    {
        public static UnitEntityDataLogicManagerComponent Instance;
        
        public Dictionary<int, List<IUnitEntityDataLogic>> ComId2Logics = new ();
    }    
}