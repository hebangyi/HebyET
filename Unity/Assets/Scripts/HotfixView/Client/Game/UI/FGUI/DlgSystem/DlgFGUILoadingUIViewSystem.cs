namespace ET.Client
{
    [EntitySystemOf(typeof(DlgFGUILoadingUIView))]
    public static partial class DlgFGUILoadingUIViewSystem
    {
        [EntitySystem]
        private static void Awake(this DlgFGUILoadingUIView self)
        {

        }
        [EntitySystem]
        private static void Update(this DlgFGUILoadingUIView self)
        {
            var sceneHandle = UnitySceneManagerComponent.Instance.UnityScene.SceneHandle;
            if (sceneHandle == null)
            {
                return;
            }
 
            if (sceneHandle.Progress < 1)
            {
                self.ShowProgress(sceneHandle.Progress - 0.3f);
                return;
            }
            
            // TODO 预加载GameObject的进度
            // TODO 时间伪加载
            
            EventSystem.Instance.PublishAsync(self.Root(), new LoadUIFinished(){UnityScene = UnitySceneManagerComponent.Instance.UnityScene}).Coroutine();
            FGUIComponent.Instance.CloseWindow(WindowID.FGUILoadingUIView);
        }


        public static void ShowProgress(this DlgFGUILoadingUIView self, float progress)
        {
            self.View.processBar.value = (int)UnitySceneManagerComponent.Instance.UnityScene.SceneHandle.Progress;
        }
        
        public static void ShowWindow(this DlgFGUILoadingUIView self, ShowWindowData showWindowData = null)
        {
            self.View.processBar.value = 0;
        }
    }
}

