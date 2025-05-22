using FairyGUI;

namespace ET.Client
{
    [FGUIEvent(WindowID.LoadingUIView, typeof(FGUILoadingUIView))]
    public class DlgFGUILoadingUIViewEventHandler: IFGUIEventHandler
    {
        public void OnInitWindowCoreData(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.WindowType = UIWindowType.Normal;
        }

        public void OnInitComponent(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.AddComponent<DlgFGUILoadingUIView>().AddComponent<FGUILoadingUIView, GObject>(uiBaseWindow.GObject);
        }

        public void OnRegisterUIEvent(UIBaseWindow uiBaseWindow)
        {
        }

        public void OnShowWindow(UIBaseWindow uiBaseWindow, ShowWindowData showWindowData = null)
        {
        }

        public void OnHideWindow(UIBaseWindow uiBaseWindow)
        {
        }

        public void BeforeUnload(UIBaseWindow uiBaseWindow)
        {
        }
    }
}
