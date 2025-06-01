using FairyGUI;

namespace ET.Client
{
    [Event(SceneType.Game)]
    public class AfterCreateCurrentUnityScene_ShowLoadingUI: AEvent<Scene, AfterCreateCurrentUnityScene>
    {
        protected override async ETTask Run(Scene scene, AfterCreateCurrentUnityScene args)
        {
            FGUIComponent.Instance.CloseWindowAll();
            await FGUIComponent.Instance.ShowWindowAsync(WindowID.LoadingUIView);
            await ETTask.CompletedTask;
        }
    }
    
    
    [FGUIEvent(typeof(FGUILoadingUIView))]
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
