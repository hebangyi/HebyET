namespace ET.Server
{
   [FriendOf(typeof(LobbyRole))]
   [EntitySystemOf(typeof(LobbyRole))]
   public static partial class LobbyRoleSystem
   {
       
       [EntitySystem]
       private static void Awake(this LobbyRole self)
       {
       }

   }
}