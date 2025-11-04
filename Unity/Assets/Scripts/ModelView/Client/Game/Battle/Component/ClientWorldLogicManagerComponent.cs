using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class ClientWorldLogicManagerComponent: Entity, IAwake
    {
        public static ClientWorldLogicManagerComponent Instance;
        
        public Dictionary<int, List<IClientEleInit>> CompId2InitViewLogics = new ();
        
        public Dictionary<int, List<IClientEleUpdate>> CompId2ElementDataUpdates = new ();
        
        public Dictionary<UETypeEnum, IClientLifeCycle> UnitEntityLifeCycles = new ();
    }
}

