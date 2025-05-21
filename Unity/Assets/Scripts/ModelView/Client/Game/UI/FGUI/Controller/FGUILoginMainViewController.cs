using FairyGUI;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf]
    [EnableMethod]
    public class FGUILoginMainViewController : Entity,IAwake
    {
        public FGUILoginMainView View { get => this.GetComponent<FGUILoginMainView>(); }

        public void RegisterUIEvent()
        {
            this.View.fgui_loginBtn.GObject.asButton.onClick.Add(OnButtonClick);
        }

        public void ShowWindow(ShowWindowData showWindowData = null)
        {
        
        }
        
        
        private void OnButtonClick(EventContext context)
        {
            string account = this.View.fgui_loginField.text;
            Log.Error(account);
            // 处理按钮点击逻辑
            Log.Info("按钮被点击了!");
        }
    }
}