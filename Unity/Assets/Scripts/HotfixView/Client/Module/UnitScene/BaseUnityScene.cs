using System;
using System.Linq;

namespace ET.Client
{
    [UnitySceneContext(UnitySceneEnum.Lobby)]
    [UnitySceneContext(UnitySceneEnum.Battle)]
    [UnitySceneContext(UnitySceneEnum.Login)]
    public class BaseUnityScene : IUnitySceneContext
    {
        public virtual void InitComponent(UnityScene unityScene)
        {
            unityScene.AddComponent<ResourcesLoaderComponent>();
            unityScene.AddComponent<GameObjectPoolComponent>();
            unityScene.AddComponent<UnitySceneCameraComponent>();
            unityScene.AddComponent<OperaComponent>();
        }
        
        public void OpenLoadingUI(UnityScene unityScene)
        {
            FGUIComponent.Instance.CloseWindowAll();
            FGUIComponent.Instance.ShowWindowAsync(WindowID.FGUILoadingUIView).Coroutine();
        }

        public void StartLoading(UnityScene unityScene)
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
                resourcesLoaderComponent.LoadScene(scenePath);
                // TODO 场景加载完成
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public void LoadingFinished(UnityScene unityScene)
        {
            // 关闭
            FGUIComponent.Instance.CloseWindow(WindowID.FGUILoadingUIView);
        }

        public void Close(UnityScene unityScene)
        {
            UnitySceneManagerComponent.Instance.UnityScene?.Dispose();
        }
    } 
}

