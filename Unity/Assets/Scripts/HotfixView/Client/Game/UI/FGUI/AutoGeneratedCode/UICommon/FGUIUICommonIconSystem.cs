//"--This is an automatically generated class by FairyGUIPlugin. Please do not modify it."
using FairyGUI;


namespace ET.Client
{
    [EntitySystemOf(typeof(FGUIUICommonIcon))]
    public static partial class FGUIUICommonIconSystem
    {
        [EntitySystem]
        public static void Awake(this FGUIUICommonIcon self, FairyGUI.GObject go)
        {
			self.UIPackageName = "UICommon";
	        self.UIResourceName = "UICommonIcon";
	        self.UIResURL = "ui://UICommon/UICommonIcon";
	        self.FUIName = "FGUIUICommonIcon";
            var com = go.asCom;
			self.fgui_n0 = (GImage)com.GetChild("n0");

        }
        
        
        [EntitySystem]
        public static void Destroy(this FGUIUICommonIcon self)
        {
			self.fgui_n0 = null;

        }
		
		// public static GObject CreateGObject(this FGUIUICommonIcon self)
		// {
		// 	return UIPackage.CreateObject(self.UIPackageName, self.UIResourceName);
		// } 

        // public static void CreateGObjectAsync(this FGUIUICommonIcon self, UIPackage.CreateObjectCallback result)
		// {
		// 	UIPackage.CreateObjectAsync(self.UIPackageName, self.UIResourceName, result);
		// }
    }
}

