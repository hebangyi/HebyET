using UnityEngine;

namespace ET.Client
{
    [ComponentOf]
    [EnableMethod]
    [FGUIDLG(WindowID.FGUIBattleOperationMainView, typeof(FGUIBattleOperationMainView))]
    public class DlgFGUIBattleOperationMainView : Entity,IAwake
    {
        public FGUIBattleOperationMainView View { get => this.GetComponent<FGUIBattleOperationMainView>(); }

        public Vector2 OnTouchBeginPoint;
        

        public float InitTouchAreaX;
        public float InitTouchAreaY;
        
        public float InitYaoGanX;
        public float InitYaoGanY;

        public float YaoGanRadius;
    }
}