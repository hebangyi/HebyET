namespace ET.Server
{
   [FriendOf(typeof(BattleRoleWorldManagerComponent))]
   [EntitySystemOf(typeof(BattleRoleWorldManagerComponent))]
   public static partial class BattleRoleWorldManagerComponentSystem
   {
       
       [EntitySystem]
       private static void Awake(this BattleRoleWorldManagerComponent self)
       {
       }

       [EntitySystem]
       private static void Destroy(this BattleRoleWorldManagerComponent self)
       {
       }

   }
}