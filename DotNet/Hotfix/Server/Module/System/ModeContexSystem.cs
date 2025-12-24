namespace ET
{
   [FriendOf(typeof(ModeContex))]
   [EntitySystemOf(typeof(ModeContex))]
   public static partial class ModeContexSystem
   {
       
       [EntitySystem]
       private static void Awake(this ModeContex self)
       {
       }

       [EntitySystem]
       private static void Destroy(this ModeContex self)
       {
       }

   }
}