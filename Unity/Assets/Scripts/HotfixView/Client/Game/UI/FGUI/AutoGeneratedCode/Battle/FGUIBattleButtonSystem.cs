//"--This is an automatically generated class by FairyGUIPlugin. Please do not modify it."
using FairyGUI;


namespace ET.Client
{
    [EntitySystemOf(typeof(FGUIBattleButton))]
    public static partial class FGUIBattleButtonSystem
    {
        [EntitySystem]
        public static void Awake(this FGUIBattleButton self, FairyGUI.GObject go)
        {
			self.UIPackageName = "Battle";
	        self.UIResourceName = "FGUIBattleButton";
	        self.UIResURL = "ui://Battle/FGUIBattleButton";
	        self.FUIName = "FGUIBattleButton";
			self.GObject = go;
            var com = go.asCom;
			self.button = (GImage)com.GetChild("button");

        }
        
        
        [EntitySystem]
        public static void Destroy(this FGUIBattleButton self)
        {
			self.button = null;

        }
    }
}

