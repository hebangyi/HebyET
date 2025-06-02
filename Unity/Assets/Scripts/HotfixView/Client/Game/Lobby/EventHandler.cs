namespace ET.Client
{
    [Event(SceneType.Game)]
    public class LoadUIFinished_Lobby: AEvent<Scene, LoadUIFinished>
    {
        protected override async ETTask Run(Scene scene, LoadUIFinished args)
        {
            if (args.UnityScene.UnitySceneType != UnitySceneType.Lobby)
            {
                return;
            }

            FGUIComponent.Instance.CloseWindowAll();
            FGUIComponent.Instance.ShowWindowAsync(WindowID.FGUILobbyMainView).Coroutine();
            await ETTask.CompletedTask;
        }
    }
}