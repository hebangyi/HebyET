namespace ET.Client
{
    
    [MessageHandler(SceneType.Game)]
    public class L2C_MatchBattleSuccessHandler: MessageHandler<Scene, L2C_MatchBattleSuccess>
    {
        protected override async ETTask Run(Scene scene, L2C_MatchBattleSuccess message)
        {
            var routerAddress = message.RouterAddress;
            var battleAddress = message.BattleAddress;
            
            Log.Info($"战斗匹配成功... {message.Token}");
            Log.Info($"路由地址 {routerAddress} 战斗服地址 {battleAddress}");
            var ret = await BattleLoginHelper.Login(scene, routerAddress, battleAddress, message.Token);
            Log.Info($"战斗服登录成功 返回码 : {ret}");

            if (ret == ErrorCode.ERR_Success)
            {
                var clientBattleSenderComponent = scene.GetComponent<ClientBattleSenderComponent>();
                C2B_PlayerGetAllAOIWorldData request = C2B_PlayerGetAllAOIWorldData.Create();
                B2C_PlayerGetAllAOIWorldData response = (B2C_PlayerGetAllAOIWorldData)await clientBattleSenderComponent.Call(request);
                
                Log.Info($"{response.ToJson()}");
            }
            await ETTask.CompletedTask;
        }
    }
}
