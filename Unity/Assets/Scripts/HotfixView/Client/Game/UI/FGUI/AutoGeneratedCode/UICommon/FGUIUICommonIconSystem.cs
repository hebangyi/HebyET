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
			self.GObject = go;
            var com = go.asCom;
			self.fgui_n1 = (GTextField)com.GetChild("n1");

        }
        
        
        [EntitySystem]
        public static void Destroy(this FGUIUICommonIcon self)
        {
			self.fgui_n1 = null;

        }
    }
}

