using System;

namespace ET.Client
{
    
    [EntitySystemOf(typeof(ClientUpdateLogicComponent))]
    [FriendOf(typeof(ClientUpdateLogicComponent))]    
    public static partial class ClientUpdateLogicComponentSystem
    {       
        [EntitySystem]
        private static void Awake(this ClientUpdateLogicComponent self)
        {
            ClientUpdateLogicComponent.Instance = self;
            ClientUpdateLogicComponent.Instance.LastFixedUpdateTime = TimeInfo.Instance.NowMillTime();
            ClientUpdateLogicComponent.Instance.LastUpdateTime = TimeInfo.Instance.NowMillTime();
        }

        [EntitySystem]
        private static void Update(this ClientUpdateLogicComponent self)
        {
            long time = TimeInfo.Instance.NowMillTime();
            var lastFixedUpdateTime = ClientUpdateLogicComponent.Instance.LastFixedUpdateTime;
            var lastUpdateTime = ClientUpdateLogicComponent.Instance.LastUpdateTime;

            long subTime = time - lastFixedUpdateTime;
            if (subTime >= GameConstant.FixedUpdateDeltaTime)
            {
                self.DoFixedUpdate(GameConstant.FixedUpdateDeltaTime);
                ClientUpdateLogicComponent.Instance.LastFixedUpdateTime += GameConstant.FixedUpdateDeltaTime;
            }

            if (time > lastUpdateTime)
            {
                self.DoUpdate(time - lastUpdateTime);
                ClientUpdateLogicComponent.Instance.LastUpdateTime = time;
            }
        }

        [EntitySystem]
        private static void Destroy(this ClientUpdateLogicComponent self)
        {
            ClientUpdateLogicComponent.Instance = null;
        }
        
        private static void DoFixedUpdate(this ClientUpdateLogicComponent self, long deltaTime)
        {
            foreach (var fixedUpdateHandler in self.FixedUpdateHandlers)
            {
                fixedUpdateHandler.Invoke(deltaTime);
            }
        }

        private static void DoUpdate(this ClientUpdateLogicComponent self, long deltaTime)
        {
            foreach (var updateHandler in self.UpdateHandlers)
            {
                updateHandler.Invoke(deltaTime);
            }
        }
        

        public static void AddFixedUpdateHandler(this ClientUpdateLogicComponent self, Action<long> func)
        {
            self.FixedUpdateHandlers.Add(func);
        }

        public static void AddUpdateHandler(this ClientUpdateLogicComponent self, Action<long> func)
        {
            self.UpdateHandlers.Add(func);
        }
    }
}

