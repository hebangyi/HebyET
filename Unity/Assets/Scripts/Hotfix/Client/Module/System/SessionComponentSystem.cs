namespace ET.Client
{
   [FriendOf(typeof(SessionComponent))]
   [EntitySystemOf(typeof(SessionComponent))]
   public static partial class SessionComponentSystem
   {
       
       [EntitySystem]
       private static void Awake(this SessionComponent self)
       {
       }

       [EntitySystem]
       private static void Destroy(this SessionComponent self)
       {
       }

   }
}