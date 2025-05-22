using FairyGUI;

namespace ET.Client
{
    
    [FGUIEvent(WindowID.CommonBG, typeof(FGUICommonBG))]
    public class DlgFGUICommonBGEventHandler: IFGUIEventHandler
    {
        public void OnInitWindowCoreData(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.WindowType = UIWindowType.Normal;
        }

        public void OnInitComponent(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.AddComponent<DlgFGUILoginMainView>().AddComponent<FGUICommonBG, GObject>(uiBaseWindow.GObject);
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
