using FairyGUI;

namespace ET.Client
{
    [FGUIEvent(WindowID.LoginMainView, typeof(FGUILoginMainView))]
    public class FGUILoginMainViewEventHandler : IFGUIEventHandler
    {
        public void OnInitWindowCoreData(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.WindowType = UIWindowType.Normal;
        }

        public void OnInitComponent(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.AddComponent<FGUILoginMainViewController>().AddComponent<FGUILoginMainView, GObject>(uiBaseWindow.GObject);
        }

        public void OnRegisterUIEvent(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.GetComponent<FGUILoginMainViewController>().RegisterUIEvent();
        }

        public void OnShowWindow(UIBaseWindow uiBaseWindow, ShowWindowData showWindowData = null)
        {
            uiBaseWindow.GetComponent<FGUILoginMainViewController>().ShowWindow(showWindowData);
        }

        public void OnHideWindow(UIBaseWindow uiBaseWindow)
        {
        }

        public void BeforeUnload(UIBaseWindow uiBaseWindow)
        {
        }
    }
}
