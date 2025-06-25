namespace ET.Client
{
    
    [MessageHandler(SceneType.Game)]
    public class L2C_MatchBattleSuccessHandler: MessageHandler<Scene, L2C_MatchBattleSuccess>
    {
        protected override async ETTask Run(Scene entity, L2C_MatchBattleSuccess message)
        {
            Log.Info($"战斗匹配成功... {message.Token} {message.Address}");
            await ETTask.CompletedTask;
        }
    }
}
