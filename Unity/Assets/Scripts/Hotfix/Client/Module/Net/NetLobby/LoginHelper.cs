using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace ET.Client
{
    public static class LoginHelper
    {
        public static async ETTask<int> Login(Scene root, string account, string password)
        {
            root.RemoveComponent<ClientLobbySenderComponent>();
            
            ClientLobbySenderComponent clientLobbySenderComponent = root.AddComponent<ClientLobbySenderComponent>();
            var (errorCode, playerId) = await clientLobbySenderComponent.LoginAsync(account, password);

            if (errorCode != ErrorCode.ERR_Success)
            {
                Log.Error($"登录错误! Error : {errorCode}");
                return errorCode;
            }
            
            root.GetComponent<PlayerComponent>().MyId = playerId;
            // 同步全局数据
            await ClientLobbyDataComponentHelper.SyncAllData(root);
            await EventSystem.Instance.PublishAsync(root, new LoginFinish());
            return ErrorCode.ERR_Success;
        }
    }
}