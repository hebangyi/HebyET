using System;
using FairyGUI;

namespace ET.Client
{
    public static class DlgFGUILoginMainViewSystem
    {
        public static void RegisterUIEvent(this DlgFGUILoginMainView self)
        {
            self.View.fgui_loginBtn.GObject.asButton.onClick.Add((EventContext context) => { self.DoLogin(context).Coroutine();});
        }

        public static void ShowWindow(this DlgFGUILoginMainView self, ShowWindowData showWindowData = null)
        {
        }

        private static async ETTask DoLogin(this DlgFGUILoginMainView self, EventContext context)
        {
            FGUIComponent.Instance.CloseWindow(WindowID.LoginMainView);
            await FGUIComponent.Instance.ShowWindowAsync(WindowID.LoadingUIView);
            
            
            /*if(self.isLogging)
            {
                return;
            }
            
            try
            {
                self.isLogging = true;
                string account = self.View.fgui_loginField.text;
                await LoginHelper.Login(self.Root(), account, "");
            }
            catch (Exception e)
            {
                Log.Error("登录失败");
                Log.Error(e);
            }
            finally
            {
                self.isLogging = false;
            }*/
        }
    }
}