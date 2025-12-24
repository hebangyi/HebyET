namespace ET.Server
{
   [FriendOf(typeof(EtcdComponent))]
   [EntitySystemOf(typeof(EtcdComponent))]
   public static partial class EtcdComponentSystem
   {
       
       [EntitySystem]
       private static void Awake(this EtcdComponent self)
       {
       }

   }
}