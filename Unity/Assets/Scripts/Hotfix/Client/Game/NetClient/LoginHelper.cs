using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace ET.Client
{
    public static class LoginHelper
    {
        public static async ETTask Login(Scene root, string account, string password)
        {
            root.RemoveComponent<ClientSenderComponent>();
            
            ClientSenderComponent clientSenderComponent = root.AddComponent<ClientSenderComponent>();
            
            long playerId = await clientSenderComponent.LoginAsync(account, password);
            root.GetComponent<PlayerComponent>().MyId = playerId;

            // 同步全局数据
            await ClientLobbyDataComponentHelper.SyncAllData(root);
            await EventSystem.Instance.PublishAsync(root, new LoginFinish());
        }
    }
}