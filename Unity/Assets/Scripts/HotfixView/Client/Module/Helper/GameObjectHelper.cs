using System;
using Spine.Unity;
using UnityEngine;
// using Spine.Unity;

namespace ET.Client
{
    public static class GameObjectHelper
    {
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

        public static async ETTask LoadUnityObject(this ClientWorld clientWorld)
        {
            string assetsName = $"Assets/Bundles/Unit/Unit.prefab";
            GameObject gameObject = await clientWorld.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(assetsName);
            clientWorld.UnitGameObject = gameObject;
        }

        public static GameObject CreateGameObjectIns(ClientWorld clientWorld, UnitEntity unitEntity)
        {
            var unitEntityCommonData = unitEntity.GetUnitEntityElemData<UnitEntityCommonData>();
            if (unitEntityCommonData == null)
            {
                Log.Error("创建 GameObject 错误, 找不到 UnitEntity UnitEntityCommonData");
                return null;
            }

            var unitEntityType = unitEntityCommonData.UnitEntityType;
            var ueLayerTypeEnum = unitEntityCommonData.UELayerTypeEnum;
            var unitGameObject = clientWorld.UnitGameObject;

            // TODO 异步创建 
            // TODO 对象池
            GameObject toGameObject = unitGameObject.Get<GameObject>(unitEntityType.ToString());
            if (toGameObject == null)
            {
                Log.Error($"创建 GameObject 错误, Unit 找不到 {unitEntityType.ToString()} 子对象");
                return null;
            }

            var parentGameObject = GlobalComponent.Instance.Default;
            switch (ueLayerTypeEnum)
            {
                case UELayerTypeEnum.Env:
                {
                    parentGameObject = GlobalComponent.Instance.Env;
                    break;
                }
                case UELayerTypeEnum.Plant:
                {
                    parentGameObject = GlobalComponent.Instance.Plant;
                    break;
                }
                case UELayerTypeEnum.Player:
                {
                    parentGameObject = GlobalComponent.Instance.Player;
                    break;
                }
                case UELayerTypeEnum.Monster:
                {
                    parentGameObject = GlobalComponent.Instance.Monster;
                    break;
                }
            }

            GameObject ins = UnityEngine.Object.Instantiate(toGameObject, parentGameObject, true);
            var unitEntityGameObjectComponent = unitEntity.TryAddComponent<UnitEntityGameObjectComponent>();
            unitEntityGameObjectComponent.GameObject = ins;
            
            ins.name = $"{unitEntityType}_{unitEntity.InsId}";

            var unitEntityPosition = unitEntity.GetUnitEntityElemData<UnitEntityPosition>();
            if (unitEntityPosition != null)
            {
                unitEntityGameObjectComponent.GameObject.transform.position =
                        new Vector3(unitEntityPosition.Position.x, unitEntityPosition.Position.y, 0);
            }

            switch (ueLayerTypeEnum)
            {
                case UELayerTypeEnum.Env:
                case UELayerTypeEnum.Player:
                case UELayerTypeEnum.Monster:
                {
                    ins.transform.rotation = Camera.main.transform.rotation;
                    break;
                }
            }

            return ins;
        }

        public static GameObject GetGameObject(this UnitEntity unitEntity)
        {
            return unitEntity.GetComponent<UnitEntityGameObjectComponent>()?.GameObject;
        }
        
        public static SkeletonAnimation GetSpineAnimation(this UnitEntity unitEntity)
        {
            return unitEntity.GetGameObject()?.Get<GameObject>("SpineAnimation")?.GetComponent<SkeletonAnimation>();
        }
    }
}