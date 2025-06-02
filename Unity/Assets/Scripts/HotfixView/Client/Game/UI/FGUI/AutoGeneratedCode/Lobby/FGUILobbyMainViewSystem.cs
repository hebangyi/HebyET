//"--This is an automatically generated class by FairyGUIPlugin. Please do not modify it."
using FairyGUI;


namespace ET.Client
{
    [EntitySystemOf(typeof(FGUILobbyMainView))]
    public static partial class FGUILobbyMainViewSystem
    {
        [EntitySystem]
        public static void Awake(this FGUILobbyMainView self, FairyGUI.GObject go)
        {
			self.UIPackageName = "Lobby";
	        self.UIResourceName = "FGUILobbyMainView";
	        self.UIResURL = "ui://Lobby/FGUILobbyMainView";
	        self.FUIName = "FGUILobbyMainView";
			self.GObject = go;
            var com = go.asCom;
			self.title = (GTextField)com.GetChild("title");

        }
        
        
        [EntitySystem]
        public static void Destroy(this FGUILobbyMainView self)
        {
			self.title = null;

        }
    }
}

