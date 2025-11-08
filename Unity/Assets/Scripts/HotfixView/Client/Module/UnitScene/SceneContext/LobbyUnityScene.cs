namespace ET.Client
{
    [UnitySceneContext(UnitySceneEnum.Lobby)]
    public class LobbyUnityScene : BaseUnityScene
    {
        public override async ETTask LoadingCompleted(UnityScene unityScene)
        {
            // 关闭
            FGUIComponent.Instance.CloseWindowAll();
            FGUIComponent.Instance.CloseWindow(WindowID.FGUILoadingUIView);
            FGUIComponent.Instance.ShowWindowAsync(WindowID.FGUILobbyMainView).Coroutine();
            
            await ETTask.CompletedTask;
        }
    }
}
