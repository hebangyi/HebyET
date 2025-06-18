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
	        self.UIResourceName = "FGUILoginMainView";
	        self.UIResURL = "ui://Login/FGUILoginMainView";
	        self.FUIName = "FGUILoginMainView";
			self.GObject = go;
            var com = go.asCom;
			self.bg = self.AddChild<FGUICommonBG,GObject>(com.GetChild("bg"));
			self.loginUserName = (GTextInput)com.GetChild("loginUserName");
			self.loginFieldBg = (GGraph)com.GetChild("loginFieldBg");
			self.loginField = (GTextInput)com.GetChild("loginField");
			self.loginBtn = (GButton)com.GetChild("loginBtn");
			self.loginBtbTitile = (GTextField)com.GetChild("loginBtbTitile");
			self.loginFieldGroup = (GGroup)com.GetChild("loginFieldGroup");

        }
        
        
        [EntitySystem]
        public static void Destroy(this FGUILoginMainView self)
        {
			self.bg?.Dispose();
			self.bg = null;
			self.loginUserName = null;
			self.loginFieldBg = null;
			self.loginField = null;
			self.loginBtn?.Dispose();
			self.loginBtn = null;
			self.loginBtbTitile = null;
			self.loginFieldGroup = null;

        }
    }
}

