namespace ET.Client
{
   [FriendOf(typeof(PlayerComponent))]
   [EntitySystemOf(typeof(PlayerComponent))]
   public static partial class PlayerComponentSystem
   {
       
       [EntitySystem]
       private static void Awake(this PlayerComponent self)
       {
       }

   }
}