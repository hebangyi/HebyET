using FairyGUI;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf]
    [EnableMethod]
    public class DlgFGUILoginMainView : Entity,IAwake
    {
        public FGUILoginMainView View { get => this.GetComponent<FGUILoginMainView>(); }

        public bool isLogging = false;
    }
}