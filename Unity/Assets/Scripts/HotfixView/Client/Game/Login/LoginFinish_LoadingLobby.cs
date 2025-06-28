namespace ET.Client
{
    // 登录完成 开始加载大厅
    [Event(SceneType.Game)]
    public class LoginFinish_LoadingLobby: AEvent<Scene, LoginFinish>
    {
        protected override async ETTask Run(Scene scene, LoginFinish a)
        {
            SceneChangeHelper.SceneChangeTo(scene, UnitySceneType.Lobby).Coroutine();
            await ETTask.CompletedTask;
        }
    }
}