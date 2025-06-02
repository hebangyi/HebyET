using FairyGUI;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf]
    [EnableMethod]
    [FGUIDLG(WindowID.FGUILoadingUIView, typeof(FGUILoadingUIView))]
    public class DlgFGUILoadingUIView : Entity,IAwake, IUpdate
    {
        public FGUILoadingUIView View { get => this.GetComponent<FGUILoadingUIView>(); }
    }
}

