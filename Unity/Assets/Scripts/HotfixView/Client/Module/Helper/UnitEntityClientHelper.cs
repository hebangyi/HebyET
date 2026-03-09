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
            if (unitEntity.HasBloodNumericalData())
            {
                // 血量条组件
                var gObject = await FGUIComponent.Instance.CreateGObject(FGUIPackage.PKG_Battle, FGUIResName.RES_Battle_FGUIHealthBar);
                unitEntity.AddComponent<UnitEntityHealthBarComponent, GObject>(gObject);
            }
        }

        /// <summary>
        /// 同步 Rotation 数据到现实 GameObject
        /// </summary>
        /// <param name="unitEntity"></param>
        public static void SyncData2Rotation(this UnitEntity unitEntity)
        {
            var unitEntityCommonData = unitEntity.GetUnitEntityElemData<UnitEntityCommonData>();
            var ueLayerTypeEnum = unitEntityCommonData.UELayerTypeEnum;

            var gameObject = unitEntity.GetGameObject();
            switch (ueLayerTypeEnum)
            {
                case UELayerTypeEnum.Env:
                case UELayerTypeEnum.Player:
                case UELayerTypeEnum.Monster:
                {
                    gameObject.transform.rotation = Camera.main.transform.rotation;
                    break;
                }
            }
        }
        
        
        /// <summary>
        /// 同步 Position 数据到 GameObject
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
        
        /// <summary>
        /// 设置unitEntity 在2D战斗上的显示排序 按Y坐标排序
        /// </summary>
        /// <param name="unitEntity"></param>
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
                    return;
                }
                meshRenderer.sortingOrder = -(int)(unitEntityPosition.Position.y * 100);
            }
            else
            {
                var spriteRenderer = unitEntity.GetSpriteRenderer();
                if (spriteRenderer != null)
                {
                    spriteRenderer.sortingOrder = -(int)(unitEntityPosition.Position.y * 100);
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
