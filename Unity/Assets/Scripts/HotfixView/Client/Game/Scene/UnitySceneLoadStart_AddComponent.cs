using System;
using UnityEngine.SceneManagement;

namespace ET.Client
{
    [Event(SceneType.Game)]
    public class UnitySceneLoadStart_AddComponent: AEvent<Scene, UnitySceneLoadStart>
    {
        protected override async ETTask Run(Scene root, UnitySceneLoadStart args)
        {
            try
            {
                var unityScene = UnitySceneManagerComponent.Instance.UnityScene;
                ResourcesLoaderComponent resourcesLoaderComponent = unityScene.GetComponent<ResourcesLoaderComponent>();
            
                // 加载场景资源
                await resourcesLoaderComponent.LoadSceneAsync($"Assets/Bundles/Scenes/{unityScene.Name}.unity", LoadSceneMode.Single);
                // 切换到map场景

                //await SceneManager.LoadSceneAsync(currentScene.Name);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

        }
    }
}