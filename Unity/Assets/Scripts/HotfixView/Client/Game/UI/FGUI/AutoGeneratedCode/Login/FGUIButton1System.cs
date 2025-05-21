//"--This is an automatically generated class by FairyGUIPlugin. Please do not modify it."
using FairyGUI;


namespace ET.Client
{
    [EntitySystemOf(typeof(FGUIButton1))]
    public static partial class FGUIButton1System
    {
        [EntitySystem]
        public static void Awake(this FGUIButton1 self, FairyGUI.GObject go)
        {
			self.UIPackageName = "Login";
	        self.UIResourceName = "Button1";
	        self.UIResURL = "ui://Login/Button1";
	        self.FUIName = "FGUIButton1";
			self.GObject = go;
            var com = go.asCom;
			self.fgui_button = com.GetController("button");
			self.fgui_n0 = (GGraph)com.GetChild("n0");
			self.fgui_n1 = (GGraph)com.GetChild("n1");
			self.fgui_n2 = (GGraph)com.GetChild("n2");

        }
        
        
        [EntitySystem]
        public static void Destroy(this FGUIButton1 self)
        {
			self.fgui_button = null;
			self.fgui_n0 = null;
			self.fgui_n1 = null;
			self.fgui_n2 = null;

        }
    }
}

