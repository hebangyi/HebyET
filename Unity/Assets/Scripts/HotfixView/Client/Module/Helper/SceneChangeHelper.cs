

using dnlib.DotNet;

namespace ET.Client
{
    public static partial class SceneChangeHelper
    {
        // 场景切换协程
        public static async ETTask SceneChangeTo(Scene root, UnitySceneType unitySceneType, params object[] ParamList)
        {
            UnitySceneManagerComponent unitySceneManagerComponent = root.GetComponent<UnitySceneManagerComponent>();
            unitySceneManagerComponent.UnityScene?.Dispose(); // 删除之前的CurrentScene，创建新的
            
            var unityScene = unitySceneManagerComponent.AddChild<UnityScene>();
            unityScene.UnitySceneType = unitySceneType;
            unityScene.ParamList = ParamList;
            unitySceneManagerComponent.UnityScene = unityScene;
            
            var afterCreateCurrentUnityScene = new AfterCreateCurrentUnityScene();
            afterCreateCurrentUnityScene.UnityScene = unityScene;
            await EventSystem.Instance.PublishAsync(root, afterCreateCurrentUnityScene);

            var unitySceneChangeStart = new UnitySceneLoadStart();
            unitySceneChangeStart.UnityScene = unityScene;
            await EventSystem.Instance.PublishAsync(root, unitySceneChangeStart);
            
            // 通知等待场景切换的协程
            // root.GetComponent<ObjectWait>().Notify(new Wait_SceneChangeFinish());
        }
    }
}