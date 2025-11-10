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
            ClientUpdateLogicComponent.Instance.LastUpdateTime = TimeInfo.Instance.NowMillTime();
        }

        [EntitySystem]
        private static void Update(this ClientUpdateLogicComponent self)
        {
            long time = TimeInfo.Instance.NowMillTime();
            var lastUpdateTime = ClientUpdateLogicComponent.Instance.LastUpdateTime;

            long subTime = time - lastUpdateTime;
            if (subTime >= GameConstant.FixedUpdateDeltaTime)
            {
                ClientUpdateLogicComponent.Instance.LastUpdateTime += GameConstant.FixedUpdateDeltaTime;
                self.DoFixedUpdate();
            }
        }

        [EntitySystem]
        private static void Destroy(this ClientUpdateLogicComponent self)
        {
            ClientUpdateLogicComponent.Instance = null;
        }
        
        private static void DoFixedUpdate(this ClientUpdateLogicComponent self)
        {
            foreach (var fixedUpdateHandler in self.FixedUpdateHandlers)
            {
                fixedUpdateHandler.Invoke(GameConstant.FixedUpdateDeltaTime);
            }
        }


        public static void AddFixedUpdateHandler(this ClientUpdateLogicComponent self, Action<long> func)
        {
            self.FixedUpdateHandlers.Add(func);
        }
    }
}

