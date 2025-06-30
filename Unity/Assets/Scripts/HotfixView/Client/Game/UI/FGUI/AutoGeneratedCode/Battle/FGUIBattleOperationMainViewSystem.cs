//"--This is an automatically generated class by FairyGUIPlugin. Please do not modify it."
using FairyGUI;


namespace ET.Client
{
    [EntitySystemOf(typeof(FGUIBattleOperationMainView))]
    public static partial class FGUIBattleOperationMainViewSystem
    {
        [EntitySystem]
        public static void Awake(this FGUIBattleOperationMainView self, FairyGUI.GObject go)
        {
			self.UIPackageName = "Battle";
	        self.UIResourceName = "FGUIBattleOperationMainView";
	        self.UIResURL = "ui://Battle/FGUIBattleOperationMainView";
	        self.FUIName = "FGUIBattleOperationMainView";
			self.GObject = go;
            var com = go.asCom;
			self.title = (GTextField)com.GetChild("title");

        }
        
        
        [EntitySystem]
        public static void Destroy(this FGUIBattleOperationMainView self)
        {
			self.title = null;

        }
    }
}

