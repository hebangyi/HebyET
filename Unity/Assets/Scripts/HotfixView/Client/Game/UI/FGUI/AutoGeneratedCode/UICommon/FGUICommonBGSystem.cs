//"--This is an automatically generated class by FairyGUIPlugin. Please do not modify it."
using FairyGUI;


namespace ET.Client
{
    [EntitySystemOf(typeof(FGUICommonBG))]
    public static partial class FGUICommonBGSystem
    {
        [EntitySystem]
        public static void Awake(this FGUICommonBG self, FairyGUI.GObject go)
        {
			self.UIPackageName = "UICommon";
	        self.UIResourceName = "CommonBG";
	        self.UIResURL = "ui://UICommon/CommonBG";
	        self.FUIName = "FGUICommonBG";
			self.GObject = go;
            var com = go.asCom;
			self.fgui_n0 = (GGraph)com.GetChild("n0");
			self.fgui_n1 = (GLoader)com.GetChild("n1");

        }
        
        
        [EntitySystem]
        public static void Destroy(this FGUICommonBG self)
        {
			self.fgui_n0 = null;
			self.fgui_n1 = null;

        }
    }
}

