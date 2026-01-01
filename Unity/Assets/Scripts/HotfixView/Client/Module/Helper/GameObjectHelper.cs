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
            
            string monsterAssetsName = $"Assets/Bundles/Unit/UnitMonster.prefab";
            GameObject monsterGameObject = await clientWorld.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(monsterAssetsName);
            clientWorld.UnitMonsterGameObject = monsterGameObject;
        }


        public static GameObject GetGameObjectIns(ClientWorld clientWorld, UELayerTypeEnum ueLayerTypeEnum, UETypeEnum ueTypeEnum, long id)
        {
            // TODO 异步创建 
            // TODO 对象池
            GameObject toGameObject = null;
            if (ueTypeEnum == UETypeEnum.Monster)
            {
                var monsterConfig = MonsterConfigCategory.Instance.GetById(id);

                if (monsterConfig == null)
                {
                    Log.Error($"创建Monster GameObject 失败, 找不到配置信息 {monsterConfig.Id}");
                    return null;    
                }
                
                toGameObject = clientWorld.UnitMonsterGameObject.Get<GameObject>(monsterConfig.AssetName);
            }
            else
            {
                var unitGameObject = clientWorld.UnitGameObject;
                toGameObject = unitGameObject.Get<GameObject>(ueTypeEnum.ToString());    
            }
            
            
            if (toGameObject == null)
            {
                Log.Error($"创建 GameObject 错误, Unit 找不到 {ueTypeEnum.ToString()} 子对象");
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

            // TODO GameObject 对象池
            GameObject ins = UnityEngine.Object.Instantiate(toGameObject, parentGameObject, true);
            return ins;
        }
        
        
        public static GameObject CreateGameObjectIns(ClientWorld clientWorld, UnitEntity unitEntity)
        {
            var unitEntityCommonData = unitEntity.GetUnitEntityElemData<UnitEntityCommonData>();

            var unitEntityType = unitEntityCommonData.UnitEntityType;
            var ueLayerTypeEnum = unitEntityCommonData.UELayerTypeEnum;

            var ins = GetGameObjectIns(clientWorld, ueLayerTypeEnum, unitEntityType, unitEntityCommonData.ConfigId);
            
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