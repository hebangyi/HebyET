//"--This is an automatically generated class by FairyGUIPlugin. Please do not modify it."
using FairyGUI;


namespace ET.Client
{
    [EntitySystemOf(typeof(FGUILoginMainView))]
    public static partial class FGUILoginMainViewSystem
    {
        [EntitySystem]
        private static void Awake(this FGUILoginMainView self, FairyGUI.GObject go)
        {
            var com = go.asCom;
			self.fgui_Test = (GButton)com.GetChild("Test");

        }
        
        
        [EntitySystem]
        private static void Destroy(this FGUILoginMainView self)
        {
			self.fgui_Test?.Dispose();
			self.fgui_Test = null;

        }
    }
}

