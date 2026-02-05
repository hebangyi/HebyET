using FairyGUI;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf]
    [EnableMethod]
    [FGUIDLG(WindowID.FGUILoginMainView, typeof(FGUILoginMainView))]
    public class DlgFGUILoginMainView : Entity,IAwake
    {
        public static DlgFGUILoginMainView Instance { get; set; }

        public FGUILoginMainView View { get => this.GetComponent<FGUILoginMainView>(); }

        public bool isLogging = false;
    }
}