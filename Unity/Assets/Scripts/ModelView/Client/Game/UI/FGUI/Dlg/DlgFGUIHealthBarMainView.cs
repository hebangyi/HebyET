namespace ET.Client
{
    [ComponentOf]
    [EnableMethod]
    [FGUIDLG(WindowID.FGUIHealthBarMainView, typeof(FGUIHealthBarMainView))]
    public class DlgFGUIHealthBarMainView : Entity,IAwake
    {
        public FGUIHealthBarMainView View { get => this.GetComponent<FGUIHealthBarMainView>(); }
        
    }
}