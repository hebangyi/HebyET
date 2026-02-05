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
            
            string playerAssetsName = $"Assets/Bundles/Unit/UnitPlayer.prefab";
            GameObject playerGameObject = await clientWorld.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(playerAssetsName);
            clientWorld.UnitPlayerGameObject = playerGameObject;
        }


        public static GameObject GetGameObjectIns(UnitEntity unitEntity,  long id)
        {
            ClientWorld clientWorld = unitEntity.ClientWorld();
            
            var unitEntityCommonData = unitEntity.GetUnitEntityElemData<UnitEntityCommonData>();
            var unitEntityType = unitEntityCommonData.UnitEntityType;
            var ueLayerTypeEnum = unitEntityCommonData.UELayerTypeEnum;
            
            
            // TODO 异步创建 
            // TODO 对象池
            GameObject toGameObject = null;
            string name = $"{unitEntityType}_{unitEntity.InsId}";
            if (unitEntityType == UETypeEnum.Monster)
            {
                var monsterConfig = MonsterConfigCategory.Instance.GetById(id);

                if (monsterConfig == null)
                {
                    Log.Error($"创建Monster GameObject 失败, 找不到配置信息 {monsterConfig.Id}");
                    return null;    
                }
                
                name = $"{unitEntityType}_{monsterConfig.Name}_{monsterConfig.AssetName}_{unitEntity.InsId}";
                toGameObject = clientWorld.UnitMonsterGameObject.Get<GameObject>(monsterConfig.AssetName);
            }
            else if (unitEntityType == UETypeEnum.Player)
            {
                var battlePlayerConfig = BattlePlayerConfigCategory.Instance.GetOne();
                
                name = $"{unitEntityType}_{battlePlayerConfig.Asset}_{unitEntity.InsId}";
                toGameObject = clientWorld.UnitPlayerGameObject.Get<GameObject>(battlePlayerConfig.Asset);
            }
            else
            {
                var unitGameObject = clientWorld.UnitGameObject;
                toGameObject = unitGameObject.Get<GameObject>(unitEntityType.ToString());    
            }
            
            
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

            // TODO GameObject 对象池
            GameObject ins = UnityEngine.Object.Instantiate(toGameObject, parentGameObject, true);
            ins.name = name;
            return ins;
        }
        
        
        public static async ETTask<GameObject> CreateGameObjectIns(ClientWorld clientWorld, UnitEntity unitEntity)
        {
            var unitEntityCommonData = unitEntity.GetUnitEntityElemData<UnitEntityCommonData>();

            var unitEntityType = unitEntityCommonData.UnitEntityType;
            var ueLayerTypeEnum = unitEntityCommonData.UELayerTypeEnum;

            var ins = GetGameObjectIns(unitEntity, unitEntityCommonData.ConfigId);
            
            // 实体组件
            var unitEntityGameObjectComponent = unitEntity.AddComponent<UnitEntityGameObjectComponent, GameObject>(ins);
            // 动画组件
            unitEntity.AddComponent<UnitEntitySpineAnimationComponent, GameObject>(ins);
            // 血量条组件
            unitEntity.AddComponent<UnitEntityHealthBarComponent>();
            
            var res = FGUIManagerComponent.Instance.GetWindowPackageAndRes(WindowID.FGUIHealthBarMainView);
            var gobject = await FGUIComponent.Instance.CreateGObject(res.Item1, res.Item2);
            FGUIComponent.Instance.AddGObjectByWindowType(UIWindowType.Normal, gobject);
            

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
            var spineAnimationObj = unitEntity.GetGameObject()?.Get<GameObject>("SpineAnimation");

            if (spineAnimationObj != null)
            {
                return spineAnimationObj.GetComponent<SkeletonAnimation>();
            }

            return null;
        }
    }
}