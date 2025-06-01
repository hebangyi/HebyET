using FairyGUI;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf]
    [EnableMethod]
    [FGUIDLG(WindowID.LoginMainView, typeof(FGUILoginMainView))]
    public class DlgFGUILoginMainView : Entity,IAwake
    {
        public FGUILoginMainView View { get => this.GetComponent<FGUILoginMainView>(); }

        public bool isLogging = false;
    }
}