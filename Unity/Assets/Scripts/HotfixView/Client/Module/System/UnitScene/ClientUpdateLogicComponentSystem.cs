namespace ET.Client
{
    
    [EntitySystemOf(typeof(ClientUpdateLogicComponent))]
    [FriendOf(typeof(ClientUpdateLogicComponent))]    
    public static partial class ClientUpdateLogicComponentSystem
    {       
        [EntitySystem]
        private static void Awake(this ClientUpdateLogicComponent self)
        {
            
        }

        [EntitySystem]
        private static void Update(this ClientUpdateLogicComponent self)
        {
            
        }
    }
}

