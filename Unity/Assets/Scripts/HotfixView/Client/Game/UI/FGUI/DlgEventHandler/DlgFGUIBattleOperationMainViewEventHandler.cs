using FairyGUI;

namespace ET.Client
{
    [FGUIEvent(typeof(FGUIBattleOperationMainView))]
    public class DlgFGUIBattleOperationMainViewEventHandler : IFGUIEventHandler
    {
        public void OnInitWindowCoreData(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.WindowType = UIWindowType.Normal;
        }

        public void OnInitComponent(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.AddComponent<DlgFGUIBattleOperationMainView>().AddComponent<FGUIBattleOperationMainView, GObject>(uiBaseWindow.GObject);
            uiBaseWindow.GetComponent<DlgFGUIBattleOperationMainView>().Init();
        }

        public void OnRegisterUIEvent(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.GetComponent<DlgFGUIBattleOperationMainView>().RegisterUIEvent();
        }

        public void OnShowWindow(UIBaseWindow uiBaseWindow, ShowWindowData showWindowData = null)
        {
            uiBaseWindow.GetComponent<DlgFGUIBattleOperationMainView>().ShowWindow(showWindowData);
        }

        public void OnHideWindow(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.GetComponent<DlgFGUIBattleOperationMainView>().HideWindow();
        }

        public void BeforeUnload(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.GetComponent<DlgFGUIBattleOperationMainView>().BeforeUnload();
        }
    }
}