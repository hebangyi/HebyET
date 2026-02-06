using System;
using FairyGUI;
using Unity.Mathematics;
using UnityEngine;

namespace ET.Client
{
    public static class DlgFGUIBattleOperationMainViewSystem
    
    {
        public static void Init(this DlgFGUIBattleOperationMainView self)
        {
            self.InitTouchAreaX = self.View.OpButton.TouchArea.x;
            self.InitTouchAreaY = self.View.OpButton.TouchArea.y;
            self.InitYaoGanX = self.View.OpButton.YaoGanImg.x;
            self.InitYaoGanY = self.View.OpButton.YaoGanImg.y;
            self.View.OpButton.TouchArea.alpha = 0f;
            self.YaoGanRadius = self.View.OpButton.TouchArea.width / 2;


            var clientWorld = UnitySceneClientWorldManagerComponent.Instance.CurrentClientWorld;
            if (clientWorld == null)
            {
                return;
            }

            foreach (var unitEntity in clientWorld.AllEntities.Values)
            {
                var unitEntityHealthBarComponent = unitEntity.GetComponent<UnitEntityHealthBarComponent>();
                var gObject = unitEntityHealthBarComponent.GObject;
                DlgFGUIBattleOperationMainView.Instance.View.GObject.asCom.AddChild(gObject);
            }
        }
        
        public static void RegisterUIEvent(this DlgFGUIBattleOperationMainView self)
        {
            self.View.OpButton.TouchArea.onTouchBegin.Add(self.OnTouchBegin);
            self.View.OpButton.TouchArea.onTouchMove.Add(self.OnTouchMove);
            self.View.OpButton.TouchArea.onTouchEnd.Add(self.OnTouchEnd);
            
            
            // 攻击按钮点击
            self.View.BattleButton.Attack.onClick.Add(self.AttackBtnOnClick);
        }

        public static void AttackBtnOnClick(this DlgFGUIBattleOperationMainView self, EventContext context)
        {
            MainPlayerHelper.OnClickAttack();
        }


        // 第一次按下按钮
        public static void OnTouchBegin(this DlgFGUIBattleOperationMainView self, EventContext context)
        {
            context.CaptureTouch();
            self.View.OpButton.YaoGanBg1.alpha = 0f;
            self.View.OpButton.YaoGanBg2.alpha = 0f;
            self.View.OpButton.YaoGanBg3.alpha = 0f;
            
            self.View.OpButton.TouchArea.alpha = 0.7f;
            
            // 转换为本地坐标
            var localPos1 = self.View.OpButton.GObject.GlobalToLocal(context.inputEvent.position);
            self.OnTouchBeginPoint = localPos1; 
            
            // 设置按钮坐标
            self.View.OpButton.YaoGanImg.SetXY(localPos1.x - self.View.OpButton.YaoGanImg.width / 2f, localPos1.y - self.View.OpButton.YaoGanImg.height / 2f);
            // 设置Touch区域坐标
            self.View.OpButton.TouchArea.SetXY(localPos1.x - self.View.OpButton.TouchArea.width / 2f, localPos1.y - self.View.OpButton.TouchArea.height / 2f);
        }

        // 拖拽
        public static void OnTouchMove(this DlgFGUIBattleOperationMainView self, EventContext context)
        {
            // 转换为本地坐标
            var localPos1 = self.View.OpButton.GObject.GlobalToLocal(context.inputEvent.position);
            var distance = Vector2.Distance(localPos1, self.OnTouchBeginPoint);
            Vector2 newPoint = new Vector2(localPos1.x, localPos1.y);
            if (distance > self.YaoGanRadius)
            {
                newPoint = self.OnTouchBeginPoint + (localPos1 - self.OnTouchBeginPoint).normalized * self.YaoGanRadius;
            }
            
            // 设置按钮坐标
            self.View.OpButton.YaoGanImg.SetXY(newPoint.x - self.View.OpButton.YaoGanImg.width / 2f, newPoint.y - self.View.OpButton.YaoGanImg.height / 2f);
            
            // + (int)self.MainCamera.transform.eulerAngles.y
            
            Vector2 indicator = newPoint - self.OnTouchBeginPoint;
            int angle = (int)(Mathf.Atan2(-indicator.y, indicator.x) * Mathf.Rad2Deg);
            self.View.angle.text = angle.ToString();
            self.lastMoveAngle = angle;

            self.SetOperation(angle);
        }

        public static void OnTouchEnd(this DlgFGUIBattleOperationMainView self, EventContext context)
        {
            self.View.OpButton.YaoGanBg1.alpha = 1f;
            self.View.OpButton.YaoGanBg2.alpha = 1f;
            self.View.OpButton.YaoGanBg3.alpha = 1f;
            
            self.View.OpButton.TouchArea.alpha = 0f;

            // 还原坐标
            self.View.OpButton.TouchArea.x = self.InitTouchAreaX;
            self.View.OpButton.TouchArea.y = self.InitTouchAreaY;
            self.View.OpButton.YaoGanImg.x = self.InitYaoGanX;
            self.View.OpButton.YaoGanImg.y = self.InitYaoGanY;
            self.lastMoveAngle = -1000;
            
            self.SetEndMoving();
        }

        public static void SetEndMoving(this DlgFGUIBattleOperationMainView self)
        {
            var operaComponent = UnitySceneManagerComponent.Instance.UnityScene?.GetComponent<OperaComponent>();
            operaComponent?.SetEndMoving();
        }

        public static void SetOperation(this DlgFGUIBattleOperationMainView self, int angle)
        {
            var operaComponent = UnitySceneManagerComponent.Instance.UnityScene?.GetComponent<OperaComponent>();
            operaComponent?.SetOperaMoveAngle(angle);
        }
        
        
        public static void ShowWindow(this DlgFGUIBattleOperationMainView self, ShowWindowData showWindowData = null)
        {
            
        }
        
        public static void HideWindow(this DlgFGUIBattleOperationMainView self)
        {
        }
        
        public static void BeforeUnload(this DlgFGUIBattleOperationMainView self)
        {
        }
    }
}