
using System.Collections.Generic;

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
            
            ClientWorld clientWorld = ClientWorldManagerComponent.Instance.CreateWorld();
            
            
            
            await clientWorld.InitWorld(response.BattleWorld, response.BattleUnitEntity);

            var unityScene = UnitySceneManagerComponent.Instance.UnityScene;
            var unitySceneCameraComponent = unityScene.GetComponent<UnitySceneCameraComponent>();
            
            // TODO 这个地方的逻辑都写在生命周期函数中
            var myPlayerUnitEntity = clientWorld.AllEntity.GetValueOrDefault(response.MyPlayerUnitEntity.InsId);
            clientWorld.MyPlayer = myPlayerUnitEntity;
            unitySceneCameraComponent.SetFlowUnitEntity(myPlayerUnitEntity);
            
            FGUIComponent.Instance.CloseWindowAll();
            FGUIComponent.Instance.ShowWindowAsync(WindowID.FGUIBattleOperationMainView).Coroutine();
        }
    }
}