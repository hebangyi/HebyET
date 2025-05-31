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
	        self.UIResourceName = "FGUICommonBG";
	        self.UIResURL = "ui://UICommon/FGUICommonBG";
	        self.FUIName = "FGUICommonBG";
			self.GObject = go;
            var com = go.asCom;
			self.bg_img = (GLoader)com.GetChild("bg_img");

        }
        
        
        [EntitySystem]
        public static void Destroy(this FGUICommonBG self)
        {
			self.bg_img = null;

        }
    }
}

