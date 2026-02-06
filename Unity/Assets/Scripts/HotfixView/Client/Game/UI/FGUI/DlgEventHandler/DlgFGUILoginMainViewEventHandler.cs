using FairyGUI;

namespace ET.Client
{
    [FGUIEvent(typeof(FGUILoginMainView))]
    public class DlgFGUILoginMainViewEventHandler : IFGUIEventHandler
    {
        public void OnInitWindowCoreData(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.WindowType = UIWindowType.Normal;
        }

        public void OnInitComponent(UIBaseWindow uiBaseWindow)
        {
            var dlgFguiLoginMainView = uiBaseWindow.AddComponent<DlgFGUILoginMainView>();
            DlgFGUILoginMainView.Instance = dlgFguiLoginMainView; 
            DlgFGUILoginMainView.Instance.AddComponent<FGUILoginMainView, GObject>(uiBaseWindow.GObject);
            DlgFGUILoginMainView.Instance.Init();
            
        }

        public void OnRegisterUIEvent(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.GetComponent<DlgFGUILoginMainView>().RegisterUIEvent();
        }

        public void OnShowWindow(UIBaseWindow uiBaseWindow, ShowWindowData showWindowData = null)
        {
            uiBaseWindow.GetComponent<DlgFGUILoginMainView>().ShowWindow(showWindowData);
        }

        public void OnHideWindow(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.GetComponent<DlgFGUILoginMainView>().HideWindow();
        }

        public void BeforeUnload(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.GetComponent<DlgFGUILoginMainView>().BeforeUnload();
        }
    }
}
