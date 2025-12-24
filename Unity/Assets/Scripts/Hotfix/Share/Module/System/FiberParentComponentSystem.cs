namespace ET
{
   [FriendOf(typeof(FiberParentComponent))]
   [EntitySystemOf(typeof(FiberParentComponent))]
   public static partial class FiberParentComponentSystem
   {
       
       [EntitySystem]
       private static void Awake(this FiberParentComponent self)
       {
       }

   }
}