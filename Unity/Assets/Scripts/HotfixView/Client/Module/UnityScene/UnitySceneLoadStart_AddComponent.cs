using System;
using System.Linq;
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
                        var matchBattleSuccess = (MatchBattleSuccess)unityScene.ParamList.First();
                        // TODO
                        var battleSceneConfig = BattleSceneConfigCategory.Instance.GetById(1001);
                        scenePath = ABPathHelper.GetScenePath(battleSceneConfig.AssetPath);
                        break;
                    }
                    default:
                    {
                        scenePath = ABPathHelper.GetScenePath(args.UnityScene.UnitySceneType.ToString());
                        break;
                    }
                }
                Log.Info("场景地址");
                Log.Info(scenePath);
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