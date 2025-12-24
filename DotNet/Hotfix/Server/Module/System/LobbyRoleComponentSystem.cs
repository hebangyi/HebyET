namespace ET.Server
{
   [FriendOf(typeof(LobbyRoleComponent))]
   [EntitySystemOf(typeof(LobbyRoleComponent))]
   public static partial class LobbyRoleComponentSystem
   {
       
       [EntitySystem]
       private static void Awake(this LobbyRoleComponent self)
       {
       }

       [EntitySystem]
       private static void Destroy(this LobbyRoleComponent self)
       {
       }

   }
}