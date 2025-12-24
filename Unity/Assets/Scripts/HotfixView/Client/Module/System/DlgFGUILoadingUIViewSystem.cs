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
            var unityScene = UnitySceneManagerComponent.Instance.UnityScene;
            if (unityScene == null)
            {
                self.ShowProgress(0);
                return;
            }

            var process = unityScene.GetLoadProcessPercent();
            self.ShowProgress(process * 1.0f / 100);
        }


        public static void ShowProgress(this DlgFGUILoadingUIView self, float progress)
        {
            self.View.processBar.value = progress;
        }
        
        public static void ShowWindow(this DlgFGUILoadingUIView self, ShowWindowData showWindowData = null)
        {
            self.View.processBar.value = 0;
        }
    }
}

