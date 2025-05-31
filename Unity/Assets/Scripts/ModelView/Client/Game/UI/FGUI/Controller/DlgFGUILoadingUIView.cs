using FairyGUI;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf]
    [EnableMethod]
    public class DlgFGUILoadingUIView : Entity,IAwake, IUpdate
    {
        public FGUILoadingUIView View { get => this.GetComponent<FGUILoadingUIView>(); }
    }
}

