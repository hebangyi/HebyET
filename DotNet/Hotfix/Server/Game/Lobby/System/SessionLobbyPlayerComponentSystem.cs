namespace ET.Server
{
    [EntitySystemOf(typeof(SessionLobbyPlayerComponent))]
    [FriendOf(typeof(SessionLobbyPlayerComponent))]
    public static partial class SessionLobbyPlayerComponentSystem
    {
        [EntitySystem]
        private static void Destroy(this SessionLobbyPlayerComponent self)
        {
            Scene root = self.Root();
            if (self.RoleId != 0)
            {
                // 玩家下线
                EventSystem.Instance.Publish(root, new LobbyRoleOffOnlineEvent { RoleId = self.RoleId});
                self.RoleId = 0;
            }
        }

        [EntitySystem]
        private static void Awake(this SessionLobbyPlayerComponent self, long roleId)
        {
            self.RoleId = roleId;
            Scene root = self.Root();
            EventSystem.Instance.Publish(root, new LobbyRoleOnlineEvent() { RoleId = roleId});
        }
    }
}