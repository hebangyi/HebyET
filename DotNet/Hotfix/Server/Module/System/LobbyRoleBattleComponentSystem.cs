namespace ET.Server
{
   [FriendOf(typeof(LobbyRoleBattleComponent))]
   [EntitySystemOf(typeof(LobbyRoleBattleComponent))]
   public static partial class LobbyRoleBattleComponentSystem
   {
       
       [EntitySystem]
       private static void Awake(this LobbyRoleBattleComponent self)
       {
       }

   }
}