using System;
using System.Collections.Generic;
using FairyGUI;
using Spine.Unity;
using UnityEngine;

namespace ET.Client
{
    public static class UnitEntityClientHelper
    {
        
        public static bool HasUnitEntityElementData<T>(this ClientUnitEntity self) where T : IUnitEntityElemData
        {
            Type type = typeof(T);
            var componentId = OpcodeType.Instance.GetOpcode(type);
            return self.UnitEntityData.ContainsKey(componentId);
        }

        public static T GetUnitEntityElemData<T>(this ClientUnitEntity self) where T : class, IUnitEntityElemData
        {
            Type type = typeof(T);
            var componentId = OpcodeType.Instance.GetOpcode(type);
            T elemData = self.UnitEntityData.GetValueOrDefault(componentId) as T;
            return elemData;
        }
        
        
        public static ClientWorld ClientWorld(this ClientUnitEntity unitEntity)
        {
            return unitEntity.GetParent<ClientWorld>();
        }




        /// <summary>
        /// 相机的旋转角度
        /// </summary>
        /// <param name="clientWorld"></param>
        /// <returns></returns>
        public static int GetCameraAngleOffSet(this ClientWorld clientWorld)
        {
            if (clientWorld.MainPlayer == null)
            {
                return 0;
            }
            
            // TODO 不要挂在MainPlayer上
            var playerCacheDataComponent = clientWorld.MainPlayer.GetComponent<MyPlayerCacheDataComponent>();
            return playerCacheDataComponent.CameraAngleOffSet % 360;
        }
        
        
        /// <summary>
        /// 同步血量数据到血量条
        /// </summary>
        /// <param name="unitEntity"></param>
        public static async ETTask AddData2HealthBar(this ClientUnitEntity unitEntity)
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
        public static void SyncData2Rotation(this ClientUnitEntity unitEntity)
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
        public static void SyncData2TransPos(this ClientUnitEntity unitEntity)
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
        public static void UpdateOrderLayer(this ClientUnitEntity unitEntity)
        {
            var unitEntityPosition = unitEntity.GetUnitEntityElemData<UnitEntityPosition>();
            if (unitEntityPosition == null)
            {
                return;
            }
            
            var cameraAngleOffSet = unitEntity.ClientWorld().GetCameraAngleOffSet();
            var angle = cameraAngleOffSet / GameConstant.Rad2Deg;

            var position = GlobalComponent.Instance.MainCamera.transform.position;
            var xValue = Math.Sin(angle) * (unitEntityPosition.Position.x - position.x);
            var yValue = Math.Cos(angle) * (unitEntityPosition.Position.y - position.y);
            
            var totalValue = xValue + yValue;
            Log.Info($"UnitEntity {unitEntity.InsId}, 更新 Order Layer : {-totalValue}");
            
            var spineAnimation = unitEntity.GetSpineAnimation();
            if (spineAnimation != null)
            {
                var meshRenderer = spineAnimation.GetComponent<MeshRenderer>();
                if (meshRenderer == null)
                {
                    return;
                }
                meshRenderer.sortingOrder = -(int)(totalValue * 100);
            }
            else
            {
                var spriteRenderer = unitEntity.GetSpriteRenderer();
                if (spriteRenderer != null)
                {
                    spriteRenderer.sortingOrder = -(int)(totalValue * 100);
                }
            }
        }
        
        public static SkeletonAnimation GetSpineAnimation(this ClientUnitEntity unitEntity)
        {
            var spineAnimationObj = unitEntity.GetGameObject()?.Get<GameObject>("SpineAnimation");

            if (spineAnimationObj != null)
            {
                return spineAnimationObj.GetComponent<SkeletonAnimation>();
            }

            return null;
        }

        public static SpriteRenderer GetSpriteRenderer(this ClientUnitEntity unitEntity)
        {
            var spriteObj = unitEntity.GetGameObject()?.Get<GameObject>("Sprite");
            if (spriteObj != null)
            {
                return spriteObj.GetComponent<SpriteRenderer>();
            }

            return null;
        }
        
        
        public static GameObject GetGameObject(this ClientUnitEntity unitEntity)
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
        
        
        public static bool HasBloodNumericalData(this ClientUnitEntity unitEntity)
        {
            if (!unitEntity.HasUnitEntityElementData<UnitEntityCommonData>() || !unitEntity.HasUnitEntityElementData<UnitEntityBloodData>())
            {
                return false;
            }

            if (!unitEntity.GetUnitEntityElemData<UnitEntityCommonData>().NumericalDatas.ContainsKey(UnitEntityNumericalTypeEnum.Blood))
            {
                return false;
            }
            return true;
        }
    }
}
