namespace ET
{
    [EntitySystemOf(typeof(World))]
    [FriendOf(typeof(World))]
    public static partial class WorldSystem
    {
        [EntitySystem]
        private static void Awake(this World self, int mode)
        {
            self.WorldMode = (WorldMode)mode;
            if (self.WorldMode == WorldMode.Logic)
            {
                self.DirtyHandler = new LogicDirtyHandler(self);
            }
        }
    }
}