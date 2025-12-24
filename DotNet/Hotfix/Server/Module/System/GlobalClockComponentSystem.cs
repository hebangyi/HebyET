namespace ET.Server
{
   [FriendOf(typeof(GlobalClockComponent))]
   [EntitySystemOf(typeof(GlobalClockComponent))]
   public static partial class GlobalClockComponentSystem
   {
       
       [EntitySystem]
       private static void Awake(this GlobalClockComponent self)
       {
       }

   }
}