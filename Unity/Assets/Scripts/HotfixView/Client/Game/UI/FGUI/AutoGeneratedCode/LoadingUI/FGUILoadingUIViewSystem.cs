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
	        self.UIResourceName = "FGUILoadingUIView";
	        self.UIResURL = "ui://LoadingUI/FGUILoadingUIView";
	        self.FUIName = "FGUILoadingUIView";
			self.GObject = go;
            var com = go.asCom;
			self.bg = self.AddChild<FGUICommonBG,GObject>(com.GetChild("bg"));
			self.processBar = (GProgressBar)com.GetChild("processBar");

        }
        
        
        [EntitySystem]
        public static void Destroy(this FGUILoadingUIView self)
        {
			self.bg?.Dispose();
			self.bg = null;
			self.processBar?.Dispose();
			self.processBar = null;

        }
    }
}

