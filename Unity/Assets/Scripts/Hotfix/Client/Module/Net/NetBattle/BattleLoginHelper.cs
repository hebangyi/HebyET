namespace ET.Client
{
    public static class BattleLoginHelper
    {
        public static async ETTask<int> Login(Scene root, string token)
        {
            root.RemoveComponent<ClientBattleSenderComponent>();

            var clientBattleSenderComponent = root.AddComponent<ClientBattleSenderComponent>();

            await ETTask.CompletedTask;
            return ErrorCode.ERR_Success;
        }
    }
}