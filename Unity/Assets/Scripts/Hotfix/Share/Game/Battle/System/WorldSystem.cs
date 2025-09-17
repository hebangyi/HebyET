namespace ET
{
    [EntitySystemOf(typeof(LogicWorld))]
    [FriendOf(typeof(LogicWorld))]
    public static partial class WorldSystem
    {
        [EntitySystem]
        private static void Awake(this LogicWorld sel)
        {
        }

        public static void InitDirtyHandler(this LogicWorld self, IDirtyHandler dirtyHandler)
        {
            self.DirtyHandler = dirtyHandler;
        }
        
        public static void InitSyncHandler(this LogicWorld self, ISyncHandler dirtyHandler)
        {
            self.SyncHandler = dirtyHandler;
        }
        
    }
}