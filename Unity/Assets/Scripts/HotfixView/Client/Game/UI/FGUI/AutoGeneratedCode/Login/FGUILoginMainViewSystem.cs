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
			self.fgui_n8 = (GGraph)com.GetChild("n8");
			self.fgui_testName = (GTextInput)com.GetChild("testName");
			self.fgui_n7 = (GTextInput)com.GetChild("n7");

        }
        
        
        [EntitySystem]
        public static void Destroy(this FGUILoginMainView self)
        {
			self.fgui_n8 = null;
			self.fgui_testName = null;
			self.fgui_n7 = null;

        }
    }
}

