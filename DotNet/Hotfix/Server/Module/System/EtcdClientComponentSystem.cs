namespace ET.Server
{
   [FriendOf(typeof(EtcdClientComponent))]
   [EntitySystemOf(typeof(EtcdClientComponent))]
   public static partial class EtcdClientComponentSystem
   {
       
       [EntitySystem]
       private static void Awake(this EtcdClientComponent self)
       {
       }

   }
}