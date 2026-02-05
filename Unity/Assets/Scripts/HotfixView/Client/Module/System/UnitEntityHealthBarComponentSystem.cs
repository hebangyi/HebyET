namespace ET.Client
{
   [FriendOf(typeof(UnitEntityHealthBarComponent))]
   [EntitySystemOf(typeof(UnitEntityHealthBarComponent))]
   public static partial class UnitEntityHealthBarComponentSystem
   {
       
       [EntitySystem]
       private static void Awake(this UnitEntityHealthBarComponent self)
       {
       }

   }
}