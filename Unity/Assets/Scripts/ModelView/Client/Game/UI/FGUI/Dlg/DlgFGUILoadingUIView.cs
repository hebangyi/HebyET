using FairyGUI;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf]
    [EnableMethod]
    [FGUIDLG(WindowID.FGUILoadingUIView, typeof(FGUILoadingUIView))]
    public class DlgFGUILoadingUIView : Entity,IAwake, IUpdate
    {
        public static DlgFGUILoadingUIView Instance { get; set; }
        
        public FGUILoadingUIView View { get => this.GetComponent<FGUILoadingUIView>(); }
    }
}

