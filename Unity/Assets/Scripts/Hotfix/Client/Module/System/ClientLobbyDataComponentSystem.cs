namespace ET.Client
{
   [FriendOf(typeof(ClientLobbyDataComponent))]
   [EntitySystemOf(typeof(ClientLobbyDataComponent))]
   public static partial class ClientLobbyDataComponentSystem
   {
       
       [EntitySystem]
       private static void Awake(this ClientLobbyDataComponent self)
       {
       }

   }
}