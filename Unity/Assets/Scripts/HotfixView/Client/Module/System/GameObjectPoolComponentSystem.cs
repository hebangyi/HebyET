using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(GameObjectPoolComponent))]
    public static partial class GameObjectPoolComponentSystem
    {
        public static void AddLoadGameObject(this GameObjectPoolComponent self, string path, long formId)
        {

            // TODO 缓存对象池
            var instanceId = IdGenerater.Instance.GenerateInstanceId();
            GameObjectLoadContext context = new GameObjectLoadContext();
            context.Path = path;
            context.FormId = formId;
            context.InstanceId = instanceId;
            self.WaitLoadingQueue.Enqueue(context);
        }


        [EntitySystem]
        private static void Update(this GameObjectPoolComponent self)
        {
            if (self.WaitLoadingQueue.Count == 0)
            {
                return;
            }

            while (self.WaitLoadingQueue.TryDequeue(out var context))
            {
                self.LodeGameObject(context).Coroutine();
                self.LoadingContext[context.InstanceId] = context;
            }
        }

        private static async ETTask LodeGameObject(this GameObjectPoolComponent self, GameObjectLoadContext context)
        {
            var unityScene = self.GetParent<UnityScene>();
            var resourcesLoaderComponent = unityScene.GetComponent<ResourcesLoaderComponent>();
            GameObject gameObject = await resourcesLoaderComponent.LoadAssetAsync<GameObject>(context.Path);
            self.LoadingContext.Remove(context.InstanceId);
            context.LoadedFinish(gameObject);
        }
        
        
        [EntitySystem]
        private static void Awake(this ET.Client.GameObjectPoolComponent self)
        {
            
        }
    }
}




