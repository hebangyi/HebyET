namespace ET.Client
{
    [Event(SceneType.Game)]
    public class LoadUIFinished_Battle: AEvent<Scene, LoadUIFinished>
    {
        protected override async ETTask Run(Scene scene, LoadUIFinished args)
        {
            if (args.UnityScene.UnitySceneType != UnitySceneType.Battle)
            {
                return;
            }
            FGUIComponent.Instance.CloseWindowAll();
            FGUIComponent.Instance.ShowWindowAsync(WindowID.FGUIBattleOperationMainView).Coroutine();
        }
    }
}

