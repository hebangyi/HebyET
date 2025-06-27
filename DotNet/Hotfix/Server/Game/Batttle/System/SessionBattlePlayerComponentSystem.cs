namespace ET.Server;

[EntitySystemOf(typeof(SessionBattlePlayerComponent))]
[FriendOf(typeof(SessionBattlePlayerComponent))]
public static partial class SessionBattlePlayerComponentSystem
{
    [EntitySystem]
    private static void Destroy(this SessionBattlePlayerComponent self)
    {
        Scene root = self.Root();
        if (self.RoleId != 0)
        {
            // 玩家下线
            EventSystem.Instance.Publish(root, new BattleRoleOffOnlineEvent { RoleId = self.RoleId});
            self.RoleId = 0;
        }
    }


    [EntitySystem]
    private static void Awake(this SessionBattlePlayerComponent self, long roleId)
    {
        self.RoleId = roleId;
        Scene root = self.Root();
        EventSystem.Instance.Publish(root, new BattleRoleOnlineEvent() { RoleId = roleId});
    }
}