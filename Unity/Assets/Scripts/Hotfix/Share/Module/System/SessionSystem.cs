namespace ET
{
   [FriendOf(typeof(Session))]
   [EntitySystemOf(typeof(Session))]
   public static partial class SessionSystem
   {
       
       [EntitySystem]
       private static void Awake(this Session self, AService p1)
       {
       }

       [EntitySystem]
       private static void Destroy(this Session self)
       {
       }

   }
}