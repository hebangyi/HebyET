namespace ET.Client
{
    [ComponentOf]
    [EnableMethod]
    [FGUIDLG(WindowID.FGUIBattleOperationMainView, typeof(FGUIBattleOperationMainView))]
    public class DlgFGUIBattleOperationMainView : Entity,IAwake
    {
        public FGUIBattleOperationMainView View { get => this.GetComponent<FGUIBattleOperationMainView>(); }
        
    }
}