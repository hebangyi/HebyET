//"--This is an automatically generated class by FairyGUIPlugin. Please do not modify it."
using FairyGUI;


namespace ET.Client
{
    [EntitySystemOf(typeof(FGUILoginMainView))]
    public static partial class FGUILoginMainViewSystem
    {
        [EntitySystem]
        public static void Awake(this FGUILoginMainView self, FairyGUI.GObject go)
        {
			self.UIPackageName = "Login";
	        self.UIResourceName = "LoginMainView";
	        self.UIResURL = "ui://Login/LoginMainView";
	        self.FUIName = "FGUILoginMainView";
			self.GObject = go;
            var com = go.asCom;
			self.fgui_bg = self.AddChild<FGUICommonBG,GObject>(com.GetChild("bg"));
			self.fgui_user_name_titime = (GTextInput)com.GetChild("user_name_titime");
			self.fgui_loginField = (GTextInput)com.GetChild("loginField");
			self.fgui_loginBtn = self.AddChild<FGUILoginBtn1,GObject>(com.GetChild("loginBtn"));
			self.fgui_login_field_group = (GGroup)com.GetChild("login_field_group");

        }
        
        
        [EntitySystem]
        public static void Destroy(this FGUILoginMainView self)
        {
			self.fgui_bg?.Dispose();
			self.fgui_bg = null;
			self.fgui_user_name_titime = null;
			self.fgui_loginField = null;
			self.fgui_loginBtn?.Dispose();
			self.fgui_loginBtn = null;
			self.fgui_login_field_group = null;

        }
    }
}

