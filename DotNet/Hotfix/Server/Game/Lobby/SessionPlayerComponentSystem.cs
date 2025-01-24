﻿namespace ET.Server
{
    [EntitySystemOf(typeof(SessionPlayerComponent))]
    [FriendOf(typeof(SessionPlayerComponent))]
    public static partial class SessionPlayerComponentSystem
    {
        [EntitySystem]
        private static void Destroy(this SessionPlayerComponent self)
        {
            Scene root = self.Root();
            if (root.IsDisposed)
            {
                return;
            }
            // 发送断线消息
            root.GetComponent<MessageLocationSenderComponent>().Get(LocationType.Unit).Send(self.RoleId, G2M_SessionDisconnect.Create());
        }

        [EntitySystem]
        private static void Awake(this SessionPlayerComponent self)
        {

        }
    }
}