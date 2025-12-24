namespace ET.Server
{
   [FriendOf(typeof(RobotCase))]
   [EntitySystemOf(typeof(RobotCase))]
   public static partial class RobotCaseSystem
   {
       
       [EntitySystem]
       private static void Awake(this RobotCase self)
       {
       }

       [EntitySystem]
       private static void Destroy(this RobotCase self)
       {
       }

   }
}