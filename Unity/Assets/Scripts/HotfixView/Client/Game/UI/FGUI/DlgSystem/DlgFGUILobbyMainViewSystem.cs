using System;
using FairyGUI;

namespace ET.Client
{
    public static class DlgFGUILobbyMainViewSystem
    {
        public static void Init(this DlgFGUILobbyMainView self)
        {
        }
        
        public static void RegisterUIEvent(this DlgFGUILobbyMainView self)
        {
            self.View.battle_btn.onClick.Add(() =>
            {
                OnClickStartBattle().Coroutine();
            });
        }
        
        public static async  ETTask OnClickStartBattle()
        {
            // ClientSenderComponent.Instance.Call()
        }
        
        public static void ShowWindow(this DlgFGUILobbyMainView self, ShowWindowData showWindowData = null)
        {
        }
        
        public static void HideWindow(this DlgFGUILobbyMainView self)
        {
        }
        
        public static void BeforeUnload(this DlgFGUILobbyMainView self)
        {
        }
    }
}