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
            
            // 同步全量数据
            C2G_GetAllDataUnits getAllDataUnits = C2G_GetAllDataUnits.Create();
            var response = (G2_GetAllDataUnits)await clientSenderComponent.Call(getAllDataUnits);
            
            await EventSystem.Instance.PublishAsync(root, new LoginFinish());
        }
    }
}