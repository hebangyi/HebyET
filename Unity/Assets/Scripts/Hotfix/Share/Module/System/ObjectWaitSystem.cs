namespace ET
{
   [FriendOf(typeof(ObjectWait))]
   [EntitySystemOf(typeof(ObjectWait))]
   public static partial class ObjectWaitSystem
   {
       
       [EntitySystem]
       private static void Awake(this ObjectWait self)
       {
       }

       [EntitySystem]
       private static void Destroy(this ObjectWait self)
       {
       }

   }
}