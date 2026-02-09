using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

namespace ET.Client
{
   [FriendOf(typeof(UnitEntityHealthBarComponent))]
   [EntitySystemOf(typeof(UnitEntityHealthBarComponent))]
   public static partial class UnitEntityHealthBarComponentSystem
   {
       
       [EntitySystem]
       private static void Awake(this UnitEntityHealthBarComponent self, GObject GObject)
       {
           self.GObject = GObject;
           self.FGUIHealthBar = self.AddComponent<FGUIHealthBar, GObject>(GObject);
           self.InitHPNumerical();
       }

       public static void InitHPNumerical(this UnitEntityHealthBarComponent self)
       {
           var unitEntity = self.GetParent<UnitEntity>();
           var unitEntityCommonData = unitEntity.GetUnitEntityElemData<UnitEntityCommonData>();
           
           var totalBlood = unitEntityCommonData.NumericalDatas.GetValueOrDefault(UnitEntityNumericalTypeEnum.Blood);
           var currentBlood = unitEntity.GetUnitEntityElemData<UnitEntityBloodData>().CurrentBlood;
           self.FGUIHealthBar.Bar.value = currentBlood;
           self.FGUIHealthBar.Bar.max = totalBlood;

           self.FGUIHealthBar.Text.text = $"{currentBlood}/{totalBlood}";
       }
       
       public static void UpdateHPBarPosition(this UnitEntityHealthBarComponent self)
       {
           var unitEntity = self.GetParent<UnitEntity>();
           var unitEntityPosition = unitEntity.GetUnitEntityElemData<UnitEntityPosition>();
           if (unitEntityPosition == null)
           {
               return;
           }

           var unitEntityGameObjectComponent = unitEntity.GetComponent<UnitEntityGameObjectComponent>();
           Vector2 screenPos = GlobalComponent.Instance.MainCamera.WorldToScreenPoint(unitEntityGameObjectComponent.GameObject.transform.position);
           Vector2 fguiPos = GRoot.inst.GlobalToLocal(new Vector2(screenPos.x, Screen.height - screenPos.y));

           var hpBar = self.GObject.asCom;
           hpBar.SetPosition(fguiPos.x - hpBar.width / 2, fguiPos.y, 0f);
       }
       
   }
}