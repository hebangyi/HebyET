using System;
using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class UpdateLogicManagerComponent: Entity, IAwake, IUpdate, IDestroy
    {
        public long LastUpdateTime = TimeInfo.Instance.NowMillTime();
        
        public long LastFixedUpdateTime = TimeInfo.Instance.NowMillTime();
        
        public  List<Action<long>> UpdateHandlers = new ();
        public  List<Action<long>> FixedUpdateHandlers = new ();

        public Queue<TaskUpdateContext> TaskUpdateQueues = new ();
        
        public static UpdateLogicManagerComponent Instance;
        
        
        public class TaskUpdateContext
        {
            public Func<ETTask> Func;
            public long MinInterval;
        }
    }
}
