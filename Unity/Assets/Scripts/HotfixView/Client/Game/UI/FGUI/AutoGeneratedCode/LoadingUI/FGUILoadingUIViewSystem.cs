//"--This is an automatically generated class by FairyGUIPlugin. Please do not modify it."
using FairyGUI;


namespace ET.Client
{
    [EntitySystemOf(typeof(FGUILoadingUIView))]
    public static partial class FGUILoadingUIViewSystem
    {
        [EntitySystem]
        public static void Awake(this FGUILoadingUIView self, FairyGUI.GObject go)
        {
			self.UIPackageName = "LoadingUI";
	        self.UIResourceName = "LoadingUIView";
	        self.UIResURL = "ui://LoadingUI/LoadingUIView";
	        self.FUIName = "FGUILoadingUIView";
			self.GObject = go;
            var com = go.asCom;
			self.fgui_n1 = (GLoader)com.GetChild("n1");

        }
        
        
        [EntitySystem]
        public static void Destroy(this FGUILoadingUIView self)
        {
			self.fgui_n1 = null;

        }
    }
}

