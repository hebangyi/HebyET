namespace ET
{
   [FriendOf(typeof(UnitEntityInitContext))]
   [EntitySystemOf(typeof(UnitEntityInitContext))]
   public static partial class UnitEntityInitContextSystem
   {
       
       [EntitySystem]
       private static void Awake(this UnitEntityInitContext self)
       {
       }

   }
}