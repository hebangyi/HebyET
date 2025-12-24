namespace ET.Server
{
   [FriendOf(typeof(LobbySyncUnitDataComponent))]
   [EntitySystemOf(typeof(LobbySyncUnitDataComponent))]
   public static partial class LobbySyncUnitDataComponentSystem
   {
       
       [EntitySystem]
       private static void Destroy(this LobbySyncUnitDataComponent self)
       {
       }

       [EntitySystem]
       private static void Awake(this LobbySyncUnitDataComponent self)
       {
       }

   }
}