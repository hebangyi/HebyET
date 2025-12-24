namespace ET
{
   [FriendOf(typeof(UnitComponent))]
   [EntitySystemOf(typeof(UnitComponent))]
   public static partial class UnitComponentSystem
   {
       
       [EntitySystem]
       private static void Awake(this UnitComponent self)
       {
       }

       [EntitySystem]
       private static void Destroy(this UnitComponent self)
       {
       }

   }
}