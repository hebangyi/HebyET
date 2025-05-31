using System;
using FairyGUI;

namespace ET.Client
{
    public static class DlgFGUILoginMainViewSystem
    {
        public static void RegisterUIEvent(this DlgFGUILoginMainView self)
        {
            self.View.loginBtn.onClick.Add((EventContext context) => { self.DoLogin(context).Coroutine();});
        }

        public static void ShowWindow(this DlgFGUILoginMainView self, ShowWindowData showWindowData = null)
        {
        }

        private static async ETTask DoLogin(this DlgFGUILoginMainView self, EventContext context)
        {
            if(self.isLogging)
            {
                return;
            }
            
            try
            {
                self.isLogging = true;
                string account = self.View.loginField.text;
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
            }
        }
    }
}