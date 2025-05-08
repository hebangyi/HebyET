//"--This is an automatically generated class by FairyGUIPlugin. Please do not modify it."
using ETModel;
using FairyGUI;

namespace ET.Client
{
    public class FGUILoginMainView: Entity, IAwake<GObject>, IDestroy
    {
        public String UIPackageName = "Login";
        public String UIResourceName = "LoginMainView";
        public String UIResURL = "ui://Login/LoginMainView";
        public String FUIName = "Login_LoginMainView";
        public System.Action<FGUILoginMainView> OnPreDisposeEvent;
//// 组件变量
		public GButton fgui_loginBtn;
		public GTextInput fgui_loginField;


    }
}