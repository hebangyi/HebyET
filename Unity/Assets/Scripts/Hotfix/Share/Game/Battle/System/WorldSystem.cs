namespace ET
{
    [EntitySystemOf(typeof(World))]
    [FriendOf(typeof(World))]
    public static partial class WorldSystem
    {
        [EntitySystem]
        private static void Awake(this World sel)
        {
        }

        public static void InitDirtyHandler(this World self, IDirtyHandler dirtyHandler)
        {
            self.DirtyHandler = dirtyHandler;
        }
        
        public static void InitSyncHandler(this World self, ISyncHandler dirtyHandler)
        {
            self.SyncHandler = dirtyHandler;
        }
        
    }
}