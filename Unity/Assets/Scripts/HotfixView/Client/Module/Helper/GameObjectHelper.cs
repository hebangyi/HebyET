using System;
using FairyGUI;
using Spine.Unity;
using UnityEngine;

// using Spine.Unity;

namespace ET.Client
{
    public static class GameObjectHelper
    {
        public static async ETTask LoadUnityObject(this ClientWorld clientWorld)
        {
            string assetsName = $"Assets/Bundles/Unit/Unit.prefab";
            GameObject gameObject = await clientWorld.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(assetsName);
            clientWorld.UnitGameObject = gameObject;

            string monsterAssetsName = $"Assets/Bundles/Unit/UnitMonster.prefab";
            GameObject monsterGameObject =
                    await clientWorld.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(monsterAssetsName);
            clientWorld.UnitMonsterGameObject = monsterGameObject;

            string playerAssetsName = $"Assets/Bundles/Unit/UnitPlayer.prefab";
            GameObject playerGameObject =
                    await clientWorld.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(playerAssetsName);
            clientWorld.UnitPlayerGameObject = playerGameObject;
        }

        public static GameObject GetGameObjectIns(UnitEntity unitEntity, long id)
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
            // var unitEntityType = unitEntityCommonData.UnitEntityType;
            var ueLayerTypeEnum = unitEntityCommonData.UELayerTypeEnum;

            var ins = GetGameObjectIns(unitEntity, unitEntityCommonData.ConfigId);

            // 实体组件
            unitEntity.AddComponent<UnitEntityGameObjectComponent, GameObject>(ins);
            // 动画组件
            unitEntity.AddComponent<UnitEntitySpineAnimationComponent, GameObject>(ins);
            
            await unitEntity.AddData2HealthBar();
            // unitEntity.SyncData2TransPos();
            // unitEntity.UpdateOrderLayer();
            
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


    }
}