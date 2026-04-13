using System;

namespace ET
{
    [FriendOf(typeof(UpdateLogicManagerComponent))]
    [EntitySystemOf(typeof(UpdateLogicManagerComponent))]
    public static partial class UpdateLogicManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UpdateLogicManagerComponent self)
        {
            UpdateLogicManagerComponent.Instance = self;
            UpdateLogicManagerComponent.Instance.LastFixedUpdateTime = TimeInfo.Instance.NowMillTime();
            UpdateLogicManagerComponent.Instance.LastUpdateTime = TimeInfo.Instance.NowMillTime();
        }

        [EntitySystem]
        private static void Update(this UpdateLogicManagerComponent self)
        {
            long time = TimeInfo.Instance.NowMillTime();
            var lastFixedUpdateTime = UpdateLogicManagerComponent.Instance.LastFixedUpdateTime;
            var lastUpdateTime = UpdateLogicManagerComponent.Instance.LastUpdateTime;

            long subTime = time - lastFixedUpdateTime;
            if (subTime >= GameConstant.FixedUpdateDeltaTime)
            {
                self.DoFixedUpdate(GameConstant.FixedUpdateDeltaTime);
                UpdateLogicManagerComponent.Instance.LastFixedUpdateTime += GameConstant.FixedUpdateDeltaTime;
            }

            if (time > lastUpdateTime)
            {
                self.DoUpdate(time - lastUpdateTime);
                UpdateLogicManagerComponent.Instance.LastUpdateTime = time;
            }

            self.DoIntervalUpdate();
        }

        [EntitySystem]
        private static void Destroy(this UpdateLogicManagerComponent self)
        {
            UpdateLogicManagerComponent.Instance = null;
        }

        /// <summary>
        /// FixedUpdate 频率更新方法 FixedUpdateDeltaTime 频率
        /// </summary>
        /// <param name="self"></param>
        /// <param name="deltaTime"></param>
        private static void DoFixedUpdate(this UpdateLogicManagerComponent self, long deltaTime)
        {
            for (int i = 0; i < self.FixedUpdateContexts.Count; i++)
            {
                if (self.FixedUpdateContexts.TryDequeue(out var context))
                {
                    if (context.IsRemove)
                    {
                        continue;
                    }
                    
                    context.deltaTime = deltaTime;
                    context.Func.Invoke(context);
                    
                    
                    self.FixedUpdateContexts.Enqueue(context);
                }
            }
        }

        /// <summary>
        /// Update频率更新方法
        /// </summary>
        /// <param name="self"></param>
        /// <param name="deltaTime"></param>
        private static void DoUpdate(this UpdateLogicManagerComponent self, long deltaTime)
        {
            for (int i = 0; i < self.UpdateContexts.Count; i++)
            {
                if (self.UpdateContexts.TryDequeue(out var context))
                {
                    if (context.IsRemove)
                    {
                        continue;
                    }
                    
                    context.deltaTime = deltaTime;
                    context.Func.Invoke(context);
                    
                    
                    self.UpdateContexts.Enqueue(context);
                }
            }
        }

        /// <summary>
        /// 方法只能同时执行一次
        /// </summary>
        /// <param name="self"></param>
        private static void DoIntervalUpdate(this UpdateLogicManagerComponent self)
        {
            for (int i = 0; i < self.TaskIntervalUpdateQueues.Count; i++)
            {
                if (self.TaskIntervalUpdateQueues.TryDequeue(out var context))
                {
                    if (context.IsRemove)
                    {
                        continue;
                    }
                    
                    ExecTaskUpdate0(self, context).Coroutine();
                }
            }
        }

        private static async ETTask ExecTaskUpdate0(this UpdateLogicManagerComponent self, UpdateLogicManagerComponent.TaskIntervalUpdateContext context)
        {
            await context.Func.Invoke(context);
            await self.Root().GetComponent<TimerComponent>().WaitAsync(context.MinInterval);
            self.TaskIntervalUpdateQueues.Enqueue(context);
        }

        public static void AddFixedUpdateFunc(this UpdateLogicManagerComponent self, Action<UpdateLogicManagerComponent.FixedUpdateContext> func)
        {
            UpdateLogicManagerComponent.FixedUpdateContext fixedUpdateContext = new ();
            fixedUpdateContext.Func = func;
            
            self.FixedUpdateContexts.Enqueue(fixedUpdateContext);
        }

        public static void AddUpdateFunc(this UpdateLogicManagerComponent self, Action<UpdateLogicManagerComponent.UpdateContext> func)
        {
            UpdateLogicManagerComponent.UpdateContext updateContext = new();
            updateContext.Func = func;
            self.UpdateContexts.Enqueue(updateContext);
        }

        /// <summary>
        /// Interval 定时器方法
        /// </summary>
        /// <param name="self"></param>
        /// <param name="func"></param>
        /// <param name="minInterval"></param>
        /// <returns></returns>
        public static UpdateLogicManagerComponent.TaskIntervalUpdateContext AddTaskUpdateFunc(this UpdateLogicManagerComponent self, Func<UpdateLogicManagerComponent.TaskIntervalUpdateContext, ETTask> func, long minInterval = 0)
        {
            UpdateLogicManagerComponent.TaskIntervalUpdateContext intervalUpdateContext = new();
            intervalUpdateContext.Func = func;
            intervalUpdateContext.MinInterval = minInterval;

            self.TaskIntervalUpdateQueues.Enqueue(intervalUpdateContext);
            return intervalUpdateContext;
        }
    }
}