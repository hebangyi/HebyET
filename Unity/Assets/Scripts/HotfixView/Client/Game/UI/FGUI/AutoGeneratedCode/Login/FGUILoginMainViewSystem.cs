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
            var com = go.asCom;
			self.fgui_testName = (GTextInput)com.GetChild("testName");
			self.fgui_loginBtn = self.AddChild<FGUILoginBtn1,GObject>(com.GetChild("loginBtn"));
			self.fgui_loginField = (GTextInput)com.GetChild("loginField");
			self.fgui_headIcon = self.AddChild<FGUIUICommonIcon,GObject>(com.GetChild("headIcon"));
        }
        
        
        [EntitySystem]
        public static void Destroy(this FGUILoginMainView self)
        {
			self.fgui_testName = null;
			self.fgui_loginBtn?.Dispose();
			self.fgui_loginBtn = null;
			self.fgui_loginField = null;
			self.fgui_headIcon?.Dispose();
			self.fgui_headIcon = null;

        }
		
		// public static GObject CreateGObject(this FGUILoginMainView self)
		// {
		// 	return UIPackage.CreateObject(self.UIPackageName, self.UIResourceName);
		// } 

        // public static void CreateGObjectAsync(this FGUILoginMainView self, UIPackage.CreateObjectCallback result)
		// {
		// 	UIPackage.CreateObjectAsync(self.UIPackageName, self.UIResourceName, result);
		// }
    }
}

