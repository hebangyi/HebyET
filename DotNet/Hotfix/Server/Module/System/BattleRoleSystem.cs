namespace ET.Server
{
   [FriendOf(typeof(BattleRole))]
   [EntitySystemOf(typeof(BattleRole))]
   public static partial class BattleRoleSystem
   {
       
       [EntitySystem]
       private static void Awake(this BattleRole self)
       {
       }

   }
}