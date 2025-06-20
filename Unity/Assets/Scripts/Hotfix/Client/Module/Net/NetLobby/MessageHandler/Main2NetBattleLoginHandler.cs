namespace ET.Client
{
    [MessageHandler(SceneType.NetBattle)]
    public class Main2NetBattleLoginHandler: MessageHandler<Scene, Main2NetBattleLogin, NetBattle2MainLogin>
    {
        protected override async ETTask Run(Scene root, Main2NetBattleLogin request, NetBattle2MainLogin response)
        {
            
            
            
            await ETTask.CompletedTask;
        }
    }
}

