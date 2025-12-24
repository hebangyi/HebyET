namespace ET.Client
{
   [FriendOf(typeof(UnitEntityAnimationComponent))]
   [EntitySystemOf(typeof(UnitEntityAnimationComponent))]
   public static partial class UnitEntityAnimationComponentSystem
   {
       
       [EntitySystem]
       private static void Awake(this UnitEntityAnimationComponent self)
       {
       }

   }
}