namespace ET.Client
{
    [ComponentOf]
    [EnableMethod]
    public class FGUILoginMainViewController : Entity,IAwake
    {
        public FGUILoginMainView View { get => this.GetComponent<FGUILoginMainView>(); }

        public void RegisterUIEvent()
        {
        
        }

        public void ShowWindow(ShowWindowData showWindowData = null)
        {
        
        }
    }
}