using System;
using System.Linq;

namespace ET.Client
{
    [UnitySceneContext(UnitySceneEnum.None)]
    public class BaseUnityScene : IUnitySceneContext
    {
        public virtual async ETTask InitComponent(UnityScene unityScene)
        {
            unityScene.AddComponent<ClientUpdateLogicComponent>();
            unityScene.AddComponent<ResourcesLoaderComponent>();
            unityScene.AddComponent<GameObjectPoolComponent>();
            unityScene.AddComponent<UnitySceneCameraComponent>();
            unityScene.AddComponent<OperaComponent>();
            unityScene.AddComponent<UnitySceneClientWorldManagerComponent>();
            unityScene.AddComponent<ClientInputComponent>(); //  输入
            
            await ETTask.CompletedTask;
        }
        
        public virtual async ETTask OpenLoadingUI(UnityScene unityScene)
        {
            FGUIComponent.Instance.CloseWindowAll();
            FGUIComponent.Instance.ShowWindowAsync(WindowID.FGUILoadingUIView).Coroutine();
            await ETTask.CompletedTask;
        }

        public virtual async ETTask StartLoading(UnityScene unityScene)
        {
            try
            {
                ResourcesLoaderComponent resourcesLoaderComponent = unityScene.GetComponent<ResourcesLoaderComponent>();
                string scenePath = "";
                switch (unityScene.UnitySceneEnum)
                {
                    case UnitySceneEnum.Battle:
                    {
                        var matchBattleSuccess = (MatchBattleSuccess)unityScene.ParamList.First();
                        var battleSceneConfig = BattleSceneConfigCategory.Instance.GetById(1001);
                        scenePath = ABPathHelper.GetScenePath(battleSceneConfig.AssetPath);
                        break;
                    }
                    default:
                    {
                        scenePath = ABPathHelper.GetScenePath(unityScene.UnitySceneEnum.ToString());
                        break;
                    }
                }
                Log.Info($"场景地址 : {scenePath}");
                
                
                // 加载场景资源
                var (task, handler) = resourcesLoaderComponent.LoadScene(scenePath);
                unityScene.LoadingSceneHandle = handler;
                UnitySceneManagerComponent.Instance.UnityScene.LoadingSuccessCallBack = () =>
                {
                    this.LoadingCompleted(unityScene).Coroutine();
                };

                await task;
                unityScene.LoadingSuccessCallBack.Invoke();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            await ETTask.CompletedTask;
        }

        public virtual async ETTask LoadingCompleted(UnityScene unityScene)
        {
            // 关闭
            FGUIComponent.Instance.CloseWindow(WindowID.FGUILoadingUIView);
            await ETTask.CompletedTask;
        }

        public virtual async ETTask Close(UnityScene unityScene)
        {
            UnitySceneManagerComponent.Instance.UnityScene?.Dispose();
            await ETTask.CompletedTask;
        }
    } 
}

