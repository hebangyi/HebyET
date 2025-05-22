using FairyGUI;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf]
    [EnableMethod]
    public class DlgFGUILoadingUIView : Entity,IAwake
    {
        public FGUILoadingUIView View { get => this.GetComponent<FGUILoadingUIView>(); }
    }
}

