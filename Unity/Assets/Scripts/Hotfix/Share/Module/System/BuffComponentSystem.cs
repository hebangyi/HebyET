namespace ET
{
   [FriendOf(typeof(BuffComponent))]
   [EntitySystemOf(typeof(BuffComponent))]
   public static partial class BuffComponentSystem
   {
       
       [EntitySystem]
       private static void Awake(this BuffComponent self)
       {
       }

   }
}