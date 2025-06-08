using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(GameObjectPoolComponent))]
    public static class GameObjectPoolComponentSystem
    {
        public static void AddLoadGameObject(this GameObjectPoolComponent self, string path, long formId)
        {

            /*var gameObjects = self.ExternalReferences.GetValueOrDefault(path);
            if (gameObjects != null)
            {
                // TODO 缓存对象
            }*/

            GameObjectLoadContext context = new GameObjectLoadContext();
            context.Path = path;
            context.FormId = formId;
            self.LoadingList.Add(context);

        }
        
        
        [EntitySystem]
        private static void Update(this ET.Client.GameObjectPoolComponent self)
        {
            if (self.LoadingList.Count == 0)
            {
                return;
            }

            // TODO 控制加载频率 量
            foreach (var item in self.LoadingList)
            {
                self.LodeGameObject(item).Coroutine();
            }
        }
        
        private static async ETTask LodeGameObject(this ET.Client.GameObjectPoolComponent self, GameObjectLoadContext context)
        {
            var unityScene = self.GetParent<UnityScene>();
            var resourcesLoaderComponent = unityScene.GetComponent<ResourcesLoaderComponent>();
            
            
            GameObject gameObject = await resourcesLoaderComponent.LoadAssetAsync<GameObject>(context.Path);
            context.DoLoadFinish(gameObject);
        }
    }
}

