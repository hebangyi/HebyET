//"--This is an automatically generated class by FairyGUIPlugin. Please do not modify it."
using FairyGUI;


namespace ET.Client
{
    [EntitySystemOf(typeof(FGUILobbyMainViewTest))]
    public static partial class FGUILobbyMainViewTestSystem
    {
        [EntitySystem]
        public static void Awake(this FGUILobbyMainViewTest self, FairyGUI.GObject go)
        {
			self.UIPackageName = "Lobby";
	        self.UIResourceName = "FGUILobbyMainViewTest";
	        self.UIResURL = "ui://Lobby/FGUILobbyMainViewTest";
	        self.FUIName = "FGUILobbyMainViewTest";
			self.GObject = go;
            var com = go.asCom;
			self.title = (GTextField)com.GetChild("title");
			self.battle_btn = (GButton)com.GetChild("battle_btn");
			self.battle_text = (GTextField)com.GetChild("battle_text");

        }
        
        
        [EntitySystem]
        public static void Destroy(this FGUILobbyMainViewTest self)
        {
			self.title = null;
			self.battle_btn?.Dispose();
			self.battle_btn = null;
			self.battle_text = null;

        }
    }
}

