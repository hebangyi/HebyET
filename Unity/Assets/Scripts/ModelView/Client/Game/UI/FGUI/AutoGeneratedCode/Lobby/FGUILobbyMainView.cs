//"--This is an automatically generated class by FairyGUIPlugin. Please do not modify it."
using System;
using FairyGUI;

namespace ET.Client
{
	[ComponentOf]
	[FGUITag(FGUIPackage.PKG_Lobby, FGUIResName.RES_Lobby_FGUILobbyMainView)]
    public class FGUILobbyMainView: FGUI, IAwake<GObject>, IDestroy
    {   
        //// 组件变量
		public GTextField title;
		public GButton battle_btn;
		public GTextField battle_text;

    }
}