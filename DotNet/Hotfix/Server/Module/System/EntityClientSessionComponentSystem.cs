namespace ET.Server
{
   [FriendOf(typeof(EntityClientSessionComponent))]
   [EntitySystemOf(typeof(EntityClientSessionComponent))]
   public static partial class EntityClientSessionComponentSystem
   {
       
       [EntitySystem]
       private static void Awake(this EntityClientSessionComponent self)
       {
       }

   }
}