using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(UnityScene))]
    public class ClientUpdateLogicComponent: Entity, IAwake, IUpdate, IDestroy
    {
        public long LastUpdateTime = TimeInfo.Instance.NowMillTime();
        
        public long LastFixedUpdateTime = TimeInfo.Instance.NowMillTime();
        
        public  List<Action<long>> UpdateHandlers = new ();
        public  List<Action<long>> FixedUpdateHandlers = new ();

        public Queue<Func<ETTask>> Queues = new ();
        
        public static ClientUpdateLogicComponent Instance;
    }
}