using System.Collections.Generic;
using UnityEngine.SceneManagement;
using YooAsset;

namespace ET.Client
{
    [EntitySystemOf(typeof(ResourcesLoaderComponent))]
    [FriendOf(typeof(ResourcesLoaderComponent))]
    public static partial class ResourcesLoaderComponentSystem
    {
        
        [EntitySystem]
        private static void Awake(this ResourcesLoaderComponent self)
        {
            self.Instance = self;
            self.package = YooAssets.GetPackage("DefaultPackage");
        }

        [EntitySystem]
        private static void Awake(this ResourcesLoaderComponent self, string packageName)
        {
            self.Instance = self;
            self.package = YooAssets.GetPackage(packageName);
        }

        [EntitySystem]
        private static void Destroy(this ResourcesLoaderComponent self)
        {
            foreach (var kv in self.handlers)
            {
                switch (kv.Value)
                {
                    case AssetHandle handle:
                        handle.Release();
                        break;
                    case AllAssetsHandle handle:
                        handle.Release();
                        break;
                    case SubAssetsHandle handle:
                        handle.Release();
                        break;
                    case RawFileHandle handle:
                        handle.Release();
                        break;
                    case SceneHandle handle:
                        if (!handle.IsMainScene())
                        {
                            handle.UnloadAsync();
                        }
                        break;
                }
            }
        }

        public static async ETTask<T> LoadAssetAsync<T>(this ResourcesLoaderComponent self, string location) where T : UnityEngine.Object
        {
            using CoroutineLock coroutineLock = await self.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.ResourcesLoader, location.GetHashCode());

            HandleBase handler;
            if (!self.handlers.TryGetValue(location, out handler))
            {
                handler = self.package.LoadAssetAsync<T>(location);

                await handler.Task;

                self.handlers.Add(location, handler);
            }

            return (T)((AssetHandle)handler).AssetObject;
        }

        public static async ETTask<Dictionary<string, T>> LoadAllAssetsAsync<T>(this ResourcesLoaderComponent self, string location) where T : UnityEngine.Object
        {
            using CoroutineLock coroutineLock = await self.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.ResourcesLoader, location.GetHashCode());

            HandleBase handler;
            if (!self.handlers.TryGetValue(location, out handler))
            {
                handler = self.package.LoadAllAssetsAsync<T>(location);
                await handler.Task;
                self.handlers.Add(location, handler);
            }

            Dictionary<string, T> dictionary = new Dictionary<string, T>();
            foreach (UnityEngine.Object assetObj in ((AllAssetsHandle)handler).AllAssetObjects)
            {
                T t = assetObj as T;
                dictionary.Add(t.name, t);
            }

            return dictionary;
        }

        /// <summary>
        /// 加载切换句柄 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="location"></param>
        /// <param name="loadSceneMode">
        /// LoadSceneMode.Single 时 Unity 会自动卸载当前所有已加载的场景
        /// LoadSceneMode.Additive 时 不会自动卸载上一个场景
        /// </param>
        /// <returns>(task,场景加载句柄)</returns>
        public static (ETTask<SceneHandle>, SceneHandle) LoadScene(this ResourcesLoaderComponent self, string location, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            var handler = self.package.LoadSceneAsync(location, loadSceneMode);
            ETTask<SceneHandle> tcs = ETTask<SceneHandle>.Create();
            handler.Completed += (h) =>
            {
                tcs.SetResult(h);
            };
            return (tcs, handler);
        }
    }
}

