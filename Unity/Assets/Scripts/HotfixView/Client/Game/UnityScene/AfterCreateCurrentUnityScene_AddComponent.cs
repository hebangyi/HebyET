namespace ET.Client
{
    [Event(SceneType.Game)]
    public class AfterCreateCurrentUnityScene_AddComponent: AEvent<Scene, AfterCreateCurrentUnityScene>
    {
        protected override async ETTask Run(Scene scene, AfterCreateCurrentUnityScene args)
        {
            var unityScene = args.UnityScene;
            unityScene.AddComponent<ResourcesLoaderComponent>();
            await ETTask.CompletedTask;
        }
    }
}