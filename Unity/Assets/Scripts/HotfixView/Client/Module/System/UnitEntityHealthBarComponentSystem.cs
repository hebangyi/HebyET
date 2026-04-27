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
           self.RefreashBar();
       }
       
       
       [EntitySystem]
       private static void Update(this UnitEntityHealthBarComponent self)
       {
           self.UpdateHPBarPosition();
       }
       
       
       public static void UpdateHPBarPosition(this UnitEntityHealthBarComponent self)
       {
           var unitEntity = self.GetParent<ClientUnitEntity>();
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

       public static void RefreashBar(this UnitEntityHealthBarComponent self)
       {
           var unitEntity = self.GetParent<ClientUnitEntity>();
           var unitEntityCommonData = unitEntity.GetUnitEntityElemData<UnitEntityCommonData>();
           
           var totalBlood = unitEntityCommonData.NumericalDatas.GetValueOrDefault(UnitEntityNumericalTypeEnum.Blood);
           var numericalData = unitEntity.GetUnitEntityElemData<UnitEntityCurrentNumericalData>();
           self.FGUIHealthBar.Bar.value = numericalData.NumericalDatas.GetValueOrDefault(UnitEntityNumericalTypeEnum.Blood);
           self.FGUIHealthBar.Bar.max = totalBlood;
           self.FGUIHealthBar.Text.text = $"{numericalData.NumericalDatas.GetValueOrDefault(UnitEntityNumericalTypeEnum.Blood)}/{totalBlood}";
       }
   }
}