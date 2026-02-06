using UnityEngine;

namespace ET.Client
{
    [ComponentOf]
    [EnableMethod]
    [FGUIDLG(WindowID.FGUIBattleOperationMainView, typeof(FGUIBattleOperationMainView))]
    public class DlgFGUIBattleOperationMainView : Entity,IAwake
    {
        public static DlgFGUIBattleOperationMainView Instance { get; set; }
        
        public FGUIBattleOperationMainView View { get => this.GetComponent<FGUIBattleOperationMainView>(); }

        public Vector2 OnTouchBeginPoint;
        

        public float InitTouchAreaX;
        public float InitTouchAreaY;
        
        public float InitYaoGanX;
        public float InitYaoGanY;

        public float YaoGanRadius;

        public int lastMoveAngle; // 操作移动的角度
    }
}