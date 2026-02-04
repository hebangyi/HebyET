//"--This is an automatically generated class by FairyGUIPlugin. Please do not modify it."
using FairyGUI;


namespace ET.Client
{
    [EntitySystemOf(typeof(FGUIHealthBarMainView))]
    public static partial class FGUIHealthBarMainViewSystem
    {
        [EntitySystem]
        public static void Awake(this FGUIHealthBarMainView self, FairyGUI.GObject go)
        {
			self.UIPackageName = "Battle";
	        self.UIResourceName = "FGUIHealthBarMainView";
	        self.UIResURL = "ui://Battle/FGUIHealthBarMainView";
	        self.FUIName = "FGUIHealthBarMainView";
			self.GObject = go;
            var com = go.asCom;
			self.Bar = (GProgressBar)com.GetChild("Bar");

        }
        
        
        [EntitySystem]
        public static void Destroy(this FGUIHealthBarMainView self)
        {
			self.Bar?.Dispose();
			self.Bar = null;

        }
    }
}

