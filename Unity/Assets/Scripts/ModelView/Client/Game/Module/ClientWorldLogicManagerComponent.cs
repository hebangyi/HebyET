using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class ClientWorldLogicManagerComponent: Entity, IAwake
    {
        public static ClientWorldLogicManagerComponent Instance;
        
        public Dictionary<int, List<IClientElemEleInit>> CompId2InitViewLogics = new ();
        
        public Dictionary<int, List<IClientElemEleUpdate>> CompId2ElementDataUpdates = new ();
        
        public Dictionary<UETypeEnum, IClientUnitEntityContext> UnitEntityContexts = new ();
    }
}

