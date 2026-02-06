using FairyGUI;

namespace ET.Client
{
    [FGUIEvent(typeof(FGUIHealthBarMainView))]
    public class DlgFGUIHealthBarMainViewEventHandler : IFGUIEventHandler
    {
        public void OnInitWindowCoreData(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.WindowType = UIWindowType.Normal;
        }

        public void OnInitComponent(UIBaseWindow uiBaseWindow)
        {
            var dlgComponent = uiBaseWindow.AddComponent<DlgFGUIHealthBarMainView>();
            DlgFGUIHealthBarMainView.Instance = dlgComponent;
            DlgFGUIHealthBarMainView.Instance.AddComponent<FGUIHealthBarMainView, GObject>(uiBaseWindow.GObject);
            DlgFGUIHealthBarMainView.Instance.Init();
        }

        public void OnRegisterUIEvent(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.GetComponent<DlgFGUIHealthBarMainView>().RegisterUIEvent();
        }

        public void OnShowWindow(UIBaseWindow uiBaseWindow, ShowWindowData showWindowData = null)
        {
            uiBaseWindow.GetComponent<DlgFGUIHealthBarMainView>().ShowWindow(showWindowData);
        }

        public void OnHideWindow(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.GetComponent<DlgFGUIHealthBarMainView>().HideWindow();
        }

        public void BeforeUnload(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.GetComponent<DlgFGUIHealthBarMainView>().BeforeUnload();
        }
    }
}