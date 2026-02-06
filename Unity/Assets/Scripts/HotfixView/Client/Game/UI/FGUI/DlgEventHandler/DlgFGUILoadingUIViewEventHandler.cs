using FairyGUI;

namespace ET.Client
{
    [FGUIEvent(typeof(FGUILoadingUIView))]
    public class DlgFGUILoadingUIViewEventHandler: IFGUIEventHandler
    {
        public void OnInitWindowCoreData(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.WindowType = UIWindowType.Normal;
        }

        public void OnInitComponent(UIBaseWindow uiBaseWindow)
        {
            var dlgComponent = uiBaseWindow.AddComponent<DlgFGUILoadingUIView>(); 
            DlgFGUILoadingUIView.Instance = dlgComponent;
            DlgFGUILoadingUIView.Instance.AddComponent<FGUILoadingUIView, GObject>(uiBaseWindow.GObject);
        }

        public void OnRegisterUIEvent(UIBaseWindow uiBaseWindow)
        {
        }

        public void OnShowWindow(UIBaseWindow uiBaseWindow, ShowWindowData showWindowData = null)
        {
            uiBaseWindow.GetComponent<DlgFGUILoadingUIView>().ShowWindow(showWindowData);
        }

        public void OnHideWindow(UIBaseWindow uiBaseWindow)
        {
        }

        public void BeforeUnload(UIBaseWindow uiBaseWindow)
        {
        }
    }
}
