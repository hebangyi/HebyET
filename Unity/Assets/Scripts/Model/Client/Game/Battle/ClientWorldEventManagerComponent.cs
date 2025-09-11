using System;
using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class ClientWorldEventManagerComponent: Entity, IAwake
    {
        public static ClientWorldEventManagerComponent Instance;
        
        
        public readonly Dictionary<Type, List<IClientWorldEvent>> AllEvents = new();
    }
}

