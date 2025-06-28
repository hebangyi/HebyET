namespace ET.Client
{
    public static class BattleLoginHelper
    {
        public static async ETTask<int> Login(Scene root, string routerAddress, string battleAddress, string token)
        {
            root.RemoveComponent<ClientBattleSenderComponent>();
            var clientBattleSenderComponent = root.AddComponent<ClientBattleSenderComponent>();
            var ret = await clientBattleSenderComponent.SessionLogin(routerAddress, battleAddress, token);
            return ret;
        }
    }
}