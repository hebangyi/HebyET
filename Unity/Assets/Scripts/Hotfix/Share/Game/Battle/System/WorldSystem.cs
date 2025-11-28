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
        
    }
}