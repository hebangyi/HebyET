//"--This is an automatically generated class by FairyGUIPlugin. Please do not modify it."
using FairyGUI;


namespace ET.Client
{
    [EntitySystemOf(typeof(FGUIHealthBar))]
    public static partial class FGUIHealthBarSystem
    {
        [EntitySystem]
        public static void Awake(this FGUIHealthBar self, FairyGUI.GObject go)
        {
			self.UIPackageName = "Battle";
	        self.UIResourceName = "FGUIHealthBar";
	        self.UIResURL = "ui://Battle/FGUIHealthBar";
	        self.FUIName = "FGUIHealthBar";
			self.GObject = go;
            var com = go.asCom;
			self.Bar = (GProgressBar)com.GetChild("Bar");
			self.Text = (GTextField)com.GetChild("Text");

        }
        
        
        [EntitySystem]
        public static void Destroy(this FGUIHealthBar self)
        {
			self.Bar?.Dispose();
			self.Bar = null;
			self.Text = null;

        }
    }
}

