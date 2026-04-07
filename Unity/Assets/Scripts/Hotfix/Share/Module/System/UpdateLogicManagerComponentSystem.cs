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

            self.DoTaskUpdate();
        }

        [EntitySystem]
        private static void Destroy(this UpdateLogicManagerComponent self)
        {
            UpdateLogicManagerComponent.Instance = null;
        }

        private static void DoFixedUpdate(this UpdateLogicManagerComponent self, long deltaTime)
        {
            foreach (var fixedUpdateHandler in self.FixedUpdateHandlers)
            {
                fixedUpdateHandler.Invoke(deltaTime);
            }
        }

        private static void DoUpdate(this UpdateLogicManagerComponent self, long deltaTime)
        {
            foreach (var updateHandler in self.UpdateHandlers)
            {
                updateHandler.Invoke(deltaTime);
            }
        }

        /// <summary>
        /// 方法只能同时执行一次
        /// </summary>
        /// <param name="self"></param>
        private static void DoTaskUpdate(this UpdateLogicManagerComponent self)
        {
            if (self.TaskUpdateQueues.TryDequeue(out var context))
            {
                ExecTaskUpdate0(self, context).Coroutine();
            }
        }

        private static async ETTask ExecTaskUpdate0(this UpdateLogicManagerComponent self, UpdateLogicManagerComponent.TaskUpdateContext context)
        {
            await context.Func.Invoke();
            await self.Root().GetComponent<TimerComponent>().WaitAsync(context.MinInterval);
            self.TaskUpdateQueues.Enqueue(context);
        }

        public static void AddFixedUpdateFunc(this UpdateLogicManagerComponent self, Action<long> func)
        {
            self.FixedUpdateHandlers.Add(func);
        }

        public static void AddUpdateFunc(this UpdateLogicManagerComponent self, Action<long> func)
        {
            self.UpdateHandlers.Add(func);
        }

        /// <summary>
        /// 方法只能同时执行一次
        /// </summary>
        /// <param name="self"></param>
        /// <param name="func"></param>
        /// <param name="minInterval">最小的时间间隔</param>
        public static void AddTaskUpdateFunc(this UpdateLogicManagerComponent self, Func<ETTask> func, long minInterval = 0)
        {
            UpdateLogicManagerComponent.TaskUpdateContext updateContext = new();
            updateContext.Func = func;
            updateContext.MinInterval = minInterval;

            self.TaskUpdateQueues.Enqueue(updateContext);
        }
    }
}