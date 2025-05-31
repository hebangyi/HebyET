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
            // self.View.fgui_processBar.
            // Log.Error("Update...");
        }


        public static void ShowWindow(this DlgFGUILoadingUIView self, ShowWindowData showWindowData = null)
        {
            self.View.processBar.value = 0;
        }
    }
}

