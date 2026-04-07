using System;

namespace ET.Client
{
    
    [EntitySystemOf(typeof(UnitSeceneUpdateLogicManagerComponent))]
    [FriendOf(typeof(UnitSeceneUpdateLogicManagerComponent))]    
    public static partial class UnitSeceneUpdateLogicManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UnitSeceneUpdateLogicManagerComponent self)
        {
            UnitSeceneUpdateLogicManagerComponent.Instance = self;
            UnitSeceneUpdateLogicManagerComponent.Instance.LastFixedUpdateTime = TimeInfo.Instance.NowMillTime();
            UnitSeceneUpdateLogicManagerComponent.Instance.LastUpdateTime = TimeInfo.Instance.NowMillTime();
        }

        [EntitySystem]
        private static void Update(this UnitSeceneUpdateLogicManagerComponent self)
        {
            long time = TimeInfo.Instance.NowMillTime();
            var lastFixedUpdateTime = UnitSeceneUpdateLogicManagerComponent.Instance.LastFixedUpdateTime;
            var lastUpdateTime = UnitSeceneUpdateLogicManagerComponent.Instance.LastUpdateTime;

            long subTime = time - lastFixedUpdateTime;
            if (subTime >= GameConstant.FixedUpdateDeltaTime)
            {
                self.DoFixedUpdate(GameConstant.FixedUpdateDeltaTime);
                UnitSeceneUpdateLogicManagerComponent.Instance.LastFixedUpdateTime += GameConstant.FixedUpdateDeltaTime;
            }

            if (time > lastUpdateTime)
            {
                self.DoUpdate(time - lastUpdateTime);
                UnitSeceneUpdateLogicManagerComponent.Instance.LastUpdateTime = time;
            }

            self.DoTaskUpdate();
        }

        [EntitySystem]
        private static void Destroy(this UnitSeceneUpdateLogicManagerComponent self)
        {
            UnitSeceneUpdateLogicManagerComponent.Instance = null;
        }
        
        private static void DoFixedUpdate(this UnitSeceneUpdateLogicManagerComponent self, long deltaTime)
        {
            foreach (var fixedUpdateHandler in self.FixedUpdateHandlers)
            {
                fixedUpdateHandler.Invoke(deltaTime);
            }
        }

        private static void DoUpdate(this UnitSeceneUpdateLogicManagerComponent self, long deltaTime)
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
        private static void DoTaskUpdate(this UnitSeceneUpdateLogicManagerComponent self)
        {
            if (self.TaskUpdateQueues.TryDequeue(out var context))
            { 
                ExecTaskUpdate0(self, context).Coroutine();
            }
        }


        private static async ETTask ExecTaskUpdate0(this UnitSeceneUpdateLogicManagerComponent self, UnitSeceneUpdateLogicManagerComponent.ClientTaskUpdateContext context)
        {
            await context.Func.Invoke();
            await self.Root().GetComponent<TimerComponent>().WaitAsync(context.MinInterval);
            self.TaskUpdateQueues.Enqueue(context);
        }
        

        public static void AddFixedUpdateFunc(this UnitSeceneUpdateLogicManagerComponent self, Action<long> func)
        {
            self.FixedUpdateHandlers.Add(func);
        }

        public static void AddUpdateFunc(this UnitSeceneUpdateLogicManagerComponent self, Action<long> func)
        {
            self.UpdateHandlers.Add(func);
        }


        /// <summary>
        /// 方法只能同时执行一次
        /// </summary>
        /// <param name="self"></param>
        /// <param name="func"></param>
        /// <param name="minInterval">最小的时间间隔</param>
        public static void AddTaskUpdateFunc(this UnitSeceneUpdateLogicManagerComponent self, Func<ETTask> func, long minInterval = 0)
        {
            UnitSeceneUpdateLogicManagerComponent.ClientTaskUpdateContext updateContext = new ();
            updateContext.Func = func;
            updateContext.MinInterval = minInterval;
            
            self.TaskUpdateQueues.Enqueue(updateContext);
        }
    }
}

