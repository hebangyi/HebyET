using System;
using System.Collections.Generic;
using System.Reflection;

namespace ET.Client
{
    [EntitySystemOf(typeof(UnitySceneManagerComponent))]
    public static partial class UnitySceneManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UnitySceneManagerComponent self)
        {
            UnitySceneManagerComponent.Instance = self;
            
            var unitySceneContexts = CodeTypes.Instance.GetAttributeTypes(typeof(UnitySceneContext));
            foreach (var type in unitySceneContexts)
            {
                var instance = Activator.CreateInstance(type);
                if (instance is IUnitySceneContext unitySceneContext)
                {
                    var attributes = type.GetCustomAttributes(typeof(UnitySceneContext), false);
                    foreach (var attribute in attributes)
                    {
                        var unitySceneContextAttr = attribute as UnitySceneContext;
                        var unitySceneEnum = unitySceneContextAttr.UnitySceneEnum;
                        self.UnitySceneContexts[unitySceneEnum] = unitySceneContext;
                    }
                }
            }
        }

        [EntitySystem]
        private static void Update(this UnitySceneManagerComponent self)
        {
            if (self.IsExecuting)
            {
                return;
            }

            if (self.UnitySceneChangeQueue.Count <= 0)
            {
                return;
            }
            
            UpdateExecute(self).Coroutine();
        }


        private static async ETTask UpdateExecute(this UnitySceneManagerComponent self)
        {
            try
            {
                self.IsExecuting = true;
                if (self.UnitySceneChangeQueue.TryDequeue(out var unitySceneChangeContext))
                {
                    await SceneChangeToAsync(self.Root(), unitySceneChangeContext.unitySceneEnum, unitySceneChangeContext.ParamList);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            finally
            {
                self.IsExecuting = false;
            }
        }
        
        private static async ETTask SceneChangeToAsync(Scene root, UnitySceneEnum unitySceneEnum, params object[] ParamList)
        {
            try
            {
                var unitySceneContext = UnitySceneManagerComponent.Instance.UnitySceneContexts.GetValueOrDefault(unitySceneEnum);
                if (unitySceneContext == null)
                {
                    Log.Warning($"Not Found Unity Scene Context, UnitySceneEnum {unitySceneEnum}");
                    unitySceneContext = UnitySceneManagerComponent.Instance.UnitySceneContexts.GetValueOrDefault(UnitySceneEnum.None);
                }
            
                // 卸载 UnityScene
                UnitySceneManagerComponent.Instance.UnityScene?.Dispose(); // 删除之前的CurrentScene，创建新的
            
                var newUnityScene = UnitySceneManagerComponent.Instance.AddChild<UnityScene>();
                newUnityScene.UnitySceneEnum = unitySceneEnum;
                newUnityScene.ParamList = ParamList;
                UnitySceneManagerComponent.Instance.UnityScene = newUnityScene;
            
                // 开启加载界面界面
                await unitySceneContext.OpenLoadingUI(newUnityScene);
                // 初始化组件
                await unitySceneContext.InitComponent(newUnityScene);
                // 开始加载场景资源
                await unitySceneContext.StartLoading(newUnityScene);
            }
            catch (Exception e)
            {
                Log.Error("切换场景中途异常");
                Log.Error(e);
                SceneChangeHelper.SceneChangeTo(root, UnitySceneEnum.Lobby).Coroutine();
            }
        }
        
    }
}

