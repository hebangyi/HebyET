//"--This is an automatically generated class by FairyGUIPlugin. Please do not modify it."
using FairyGUI;


namespace ET.Client
{
    [EntitySystemOf(typeof(FGUILoginMainView))]
    public static partial class FGUILoginMainViewSystem
    {
        [EntitySystem]
        private static void Awake(this FGUILoginMainView self, FairyGUI.GObject go)
        {
            var com = go.asCom;
			self.fgui_testName = (GTextInput)com.GetChild("testName");
			self.fgui_loginBtn = (GButton)com.GetChild("loginBtn");
			self.fgui_loginField = (GTextInput)com.GetChild("loginField");
			self.fgui_testImage = (GImage)com.GetChild("testImage");

        }
        
        
        [EntitySystem]
        private static void Destroy(this FGUILoginMainView self)
        {
			self.fgui_testName = null;
			self.fgui_loginBtn?.Dispose();
			self.fgui_loginBtn = null;
			self.fgui_loginField = null;
			self.fgui_testImage = null;

        }
    }
}

