namespace ET.Client
{
    [Event(SceneType.Game)]
    public class MatchBattleSuccess_ChangeScene: AEvent<Scene, MatchBattleSuccess>
    {
        protected override async ETTask Run(Scene scene, MatchBattleSuccess args)
        {            
            SceneChangeHelper.SceneChangeTo(scene, UnitySceneType.Battle, args).Coroutine();
            await ETTask.CompletedTask;
        }
    }
}
