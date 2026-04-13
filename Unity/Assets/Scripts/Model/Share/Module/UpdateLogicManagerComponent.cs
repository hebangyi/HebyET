using System;
using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class UpdateLogicManagerComponent: Entity, IAwake, IUpdate, IDestroy
    {
        public long LastUpdateTime = TimeInfo.Instance.NowMillTime();
        
        public long LastFixedUpdateTime = TimeInfo.Instance.NowMillTime();
        
        public Queue<UpdateContext> UpdateContexts = new ();
        public Queue<FixedUpdateContext> FixedUpdateContexts = new ();
        public Queue<TaskIntervalUpdateContext> TaskIntervalUpdateQueues = new ();
        
        
        public static UpdateLogicManagerComponent Instance;
        
        public class FixedUpdateContext
        {
            public bool IsRemove = false;
            public Action<FixedUpdateContext> Func;
            public long deltaTime;
        }
        
        
        public class UpdateContext
        {
            public bool IsRemove = false;
            public Action<UpdateContext> Func;
            public long deltaTime;
        }
        
        public class TaskIntervalUpdateContext
        {
            public bool IsRemove = false;
            public Func<TaskIntervalUpdateContext, ETTask> Func;
            public long MinInterval;
        }
    }
}
