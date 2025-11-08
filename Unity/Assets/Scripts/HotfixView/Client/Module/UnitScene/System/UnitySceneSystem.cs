namespace ET.Client
{

    [EntitySystemOf(typeof(UnityScene))]
    [FriendOf(typeof(UnityScene))]
    public static partial class UnitySceneSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.UnityScene self)
        {
        }
        
        [EntitySystem]
        private static void Destroy(this ET.Client.UnityScene self)
        {
            self.LoadingSceneHandle = null;
        }

        
        public static int GetLoadProcessPercent(this ET.Client.UnityScene self)
        {
            var sceneHandle = self.LoadingSceneHandle;
            if (sceneHandle == null)
            {
                return 0;
            }

            if (sceneHandle.Progress < 1)
            {
                return (int)(sceneHandle.Progress - 0.3f) * 100;
            }
            
            return 100;
        }
    }
}

