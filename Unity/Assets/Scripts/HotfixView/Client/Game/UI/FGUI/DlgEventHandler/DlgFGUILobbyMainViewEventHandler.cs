using FairyGUI;

namespace ET.Client
{
    [FGUIEvent(typeof(FGUILobbyMainView))]
    public class DlgFGUILobbyMainViewEventHandler : IFGUIEventHandler
    {
        public void OnInitWindowCoreData(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.WindowType = UIWindowType.Normal;
        }

        public void OnInitComponent(UIBaseWindow uiBaseWindow)
        {
            var dlgComponent = uiBaseWindow.AddComponent<DlgFGUILobbyMainView>(); 
            DlgFGUILobbyMainView.Instance = dlgComponent;
            DlgFGUILobbyMainView.Instance.AddComponent<FGUILobbyMainView, GObject>(uiBaseWindow.GObject);
            DlgFGUILobbyMainView.Instance.Init();
        }

        public void OnRegisterUIEvent(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.GetComponent<DlgFGUILobbyMainView>().RegisterUIEvent();
        }

        public void OnShowWindow(UIBaseWindow uiBaseWindow, ShowWindowData showWindowData = null)
        {
            uiBaseWindow.GetComponent<DlgFGUILobbyMainView>().ShowWindow(showWindowData);
        }

        public void OnHideWindow(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.GetComponent<DlgFGUILobbyMainView>().HideWindow();
        }

        public void BeforeUnload(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.GetComponent<DlgFGUILobbyMainView>().BeforeUnload();
        }
    }
}