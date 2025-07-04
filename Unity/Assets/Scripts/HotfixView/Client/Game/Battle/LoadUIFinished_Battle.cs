namespace ET.Client
{
    [Event(SceneType.Game)]
    public class LoadUIFinished_Battle: AEvent<Scene, LoadUIFinished>
    {
        protected override async ETTask Run(Scene scene, LoadUIFinished args)
        {
            if (args.UnityScene.UnitySceneType != UnitySceneType.Battle)
            {
                return;
            }
            
            var clientBattleSenderComponent = scene.GetComponent<ClientBattleSenderComponent>();
            C2B_PlayerGetAllAOIWorldData request = C2B_PlayerGetAllAOIWorldData.Create();
            B2C_PlayerGetAllAOIWorldData response = (B2C_PlayerGetAllAOIWorldData)await clientBattleSenderComponent.Call(request);
            var unityScene = args.UnityScene;
            
            
            
            FGUIComponent.Instance.CloseWindowAll();
            FGUIComponent.Instance.ShowWindowAsync(WindowID.FGUIBattleOperationMainView).Coroutine();
        }
    }
}