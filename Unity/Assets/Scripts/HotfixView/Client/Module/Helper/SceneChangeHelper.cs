

using System.Collections.Generic;
using dnlib.DotNet;

namespace ET.Client
{
    public static partial class SceneChangeHelper
    {
        // 场景切换协程
        public static async ETTask SceneChangeTo(Scene root, UnitySceneEnum unitySceneEnum, params object[] ParamList)
        {
            var unitySceneContext = UnitySceneManagerComponent.Instance.UnitySceneContexts.GetValueOrDefault(unitySceneEnum);
            if (unitySceneContext == null)
            {
                Log.Error($"Not Found Unity Scene Context, UnitySceneEnum {unitySceneEnum}");
                return;
            }
            
            // 卸载 UnityScene
            UnitySceneManagerComponent.Instance.UnityScene?.Dispose(); // 删除之前的CurrentScene，创建新的
            
            var newUnityScene = UnitySceneManagerComponent.Instance.AddChild<UnityScene>();
            newUnityScene.UnitySceneEnum = unitySceneEnum;
            newUnityScene.ParamList = ParamList;
            UnitySceneManagerComponent.Instance.UnityScene = newUnityScene;
            
            // 开启加载界面界面
            unitySceneContext.OpenLoadingUI(newUnityScene);
            // 初始化组件
            unitySceneContext.InitComponent(newUnityScene);
            // 开始加载场景资源
            unitySceneContext.StartLoading(newUnityScene);
        }
    }
}