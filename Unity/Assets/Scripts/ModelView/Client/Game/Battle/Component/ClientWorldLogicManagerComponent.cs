using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class ClientWorldLogicManagerComponent: Entity, IAwake
    {
        public static ClientWorldLogicManagerComponent Instance;
        
        public Dictionary<int, List<IClientWorldLogicInitLogic>> CompId2InitViewLogics = new ();
        
        public Dictionary<int, List<IUnitEntityClientWorldElementDataUpdateLogic>> CompId2ElementDataUpdates = new ();
    }
}

