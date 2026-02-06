namespace ET.Client
{
    [ComponentOf]
    [EnableMethod]
    [FGUIDLG(WindowID.FGUILobbyMainView, typeof(FGUILobbyMainView))]
    public class DlgFGUILobbyMainView : Entity,IAwake
    {
        public static DlgFGUILobbyMainView Instance { get; set; }
        
        public FGUILobbyMainView View { get => this.GetComponent<FGUILobbyMainView>(); }
        
    }
}