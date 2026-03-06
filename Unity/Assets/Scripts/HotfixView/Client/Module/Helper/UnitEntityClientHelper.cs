using System;
using FairyGUI;
using Spine.Unity;
using UnityEngine;

namespace ET.Client
{
    public static class UnitEntityClientHelper
    {
        /// <summary>
        /// 同步血量数据到血量条
        /// </summary>
        /// <param name="unitEntity"></param>
        public static async ETTask AddData2HealthBar(this UnitEntity unitEntity)
        {
            if (unitEntity.HasBloodNumerical())
            {
                // 血量条组件
                var gObject = await FGUIComponent.Instance.CreateGObject(FGUIPackage.PKG_Battle, FGUIResName.RES_Battle_FGUIHealthBar);
                unitEntity.AddComponent<UnitEntityHealthBarComponent, GObject>(gObject);
            }
        }
        
        /// <summary>
        /// 同步Position数据到现实
        /// </summary>
        /// <param name="unitEntity"></param>
        public static void SyncData2TransPos(this UnitEntity unitEntity)
        {
            var unitEntityGameObjectComponent = unitEntity.GetComponent<UnitEntityGameObjectComponent>();
            var unitEntityPosition = unitEntity.GetUnitEntityElemData<UnitEntityPosition>();
            if (unitEntityPosition != null)
            {
                unitEntityGameObjectComponent.GameObject.transform.position =
                        new Vector3(unitEntityPosition.Position.x, unitEntityPosition.Position.y, 0);
            }
        }
        
        
        public static void UpdateOrderLayer(this UnitEntity unitEntity)
        {
            var unitEntityPosition = unitEntity.GetUnitEntityElemData<UnitEntityPosition>();
            if (unitEntityPosition == null)
            {
                return;
            }
            
            var spineAnimation = unitEntity.GetSpineAnimation();
            if (spineAnimation != null)
            {
                var meshRenderer = spineAnimation.GetComponent<MeshRenderer>();
                if (meshRenderer == null)
                {
                    Log.Info("meshRenderer is null");
                    return;
                }
                meshRenderer.sortingOrder = -(int)(unitEntityPosition.Position.y * 100);
                Log.Info($"修改 UnitEntity Order Layer {meshRenderer.sortingOrder}");
                //spineAnimation.order
            }
            else
            {
                var spriteRenderer = unitEntity.GetSpriteRenderer();
                if (spriteRenderer != null)
                {
                    spriteRenderer.sortingOrder = -(int)(unitEntityPosition.Position.y * 100);
                    Log.Info($"修改 UnitEntity Order Layer {spriteRenderer.sortingOrder}");
                }
            }
        }
        
        public static SkeletonAnimation GetSpineAnimation(this UnitEntity unitEntity)
        {
            var spineAnimationObj = unitEntity.GetGameObject()?.Get<GameObject>("SpineAnimation");

            if (spineAnimationObj != null)
            {
                return spineAnimationObj.GetComponent<SkeletonAnimation>();
            }

            return null;
        }

        public static SpriteRenderer GetSpriteRenderer(this UnitEntity unitEntity)
        {
            var spriteObj = unitEntity.GetGameObject()?.Get<GameObject>("Sprite");
            if (spriteObj != null)
            {
                return spriteObj.GetComponent<SpriteRenderer>();
            }

            return null;
        }
        
        
        public static GameObject GetGameObject(this UnitEntity unitEntity)
        {
            return unitEntity.GetComponent<UnitEntityGameObjectComponent>()?.GameObject;
        }
        
        
        public static T Get<T>(this GameObject gameObject, string key) where T : class
        {
            try
            {
                return gameObject.GetComponent<ReferenceCollector>()?.Get<T>(key);
            }
            catch (Exception e)
            {
                throw new Exception($"获取{gameObject.name}的ReferenceCollector key失败, key: {key}", e);
            }
        }
    }
}
