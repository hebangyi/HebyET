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
            self.View.battle_btn.onClick.Add((EventContext context) => { OnClickStartBattle().Coroutine(); });
        }

        public static async ETTask OnClickStartBattle()
        {
            Log.Info("OnClick StartBattle");
            var request = C2L_StartMatchBattle.Create();
            var response = await ClientLobbySenderComponent.Instance.Call(request) as L2C_StartMatchBattle;
            if (response == null)
            {
                return;
            }

            if (response.Error != ErrorCode.ERR_Success)
            {
                Log.Error(response.Error.ToString());
                return;
            }
            // ClientSenderComponent.Instance.Call()
            await ETTask.CompletedTask;
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