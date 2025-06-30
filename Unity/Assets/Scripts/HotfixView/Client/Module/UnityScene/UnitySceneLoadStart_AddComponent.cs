using System;
using UnityEngine;
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

                string scenePath = "";
                switch (unityScene.UnitySceneType)
                {
                    case UnitySceneType.Battle:
                    {
                        
                        
                        
                        // TODO
                        break;
                    }
                    default:
                    {
                        scenePath = ABPathHelper.GetScenePath(args.UnityScene.UnitySceneType.ToString());
                        break;
                    }
                }
                
                // 加载场景资源
                await resourcesLoaderComponent.LoadSceneAsync(scenePath);
                
                // TODO 场景加载完成
                
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

        }
    }
}