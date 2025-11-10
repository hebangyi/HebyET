using System;
using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(UnityScene))]
    public class ClientUpdateLogicComponent: Entity, IAwake, IUpdate, IDestroy
    {
        public long LastUpdateTime = TimeInfo.Instance.NowMillTime();
        
        public  List<Action<long>> FixedUpdateHandlers = new List<Action<long>>();
        
        public static ClientUpdateLogicComponent Instance;
    }
}