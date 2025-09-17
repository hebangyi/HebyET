using System;
using UnityEngine;

namespace ET.Client
{
	public static class GameObjectHelper
	{
		public static T Get<T>(this GameObject gameObject, string key) where T : class
		{
			try
			{
				return gameObject.GetComponent<ReferenceCollector>().Get<T>(key);
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

		public static async ETTask<GameObject> CreateGameObjectIns(ClientWorld clientWorld, UnitEntity unitEntity)
		{
			var unitEntityCommonData = unitEntity.GetUnitEntityElemData<UnitEntityCommonData>();

			if (unitEntityCommonData == null)
			{
				Log.Error("创建 GameObject 错误, 找不到 UnitEntity UnitEntityCommonData");
				return null;
			}

			var unitEntityType = unitEntityCommonData.UnitEntityType;
			var unitGameObject = clientWorld.UnitGameObject;

			GameObject toGameObject = unitGameObject.Get<GameObject>(unitEntityType.ToString());
			if (toGameObject == null)
			{
				Log.Error($"创建 GameObject 错误, Unit 找不到 {unitEntityType.ToString()} 子对象");
				return null;
			}
			
			GameObject ins = UnityEngine.Object.Instantiate(toGameObject, GlobalComponent.Instance.Unit, true);
			
			var unitEntityGameObjectComponent = unitEntity.TryAddComponent<UnitEntityGameObjectComponent>();
			unitEntityGameObjectComponent.GameObject = ins;
			return ins;
		}
	}
}