using System.Collections.Generic;
using System.Linq;

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
            
            World world = BattleClientWorldManagerComponent.Instance.CreateWorld();
            world.InitWorld(response.BattleWorld, response.BattleUnitEntity);

            var unityScene = UnitySceneManagerComponent.Instance.UnityScene;
            var unitySceneCameraComponent = unityScene.GetComponent<UnitySceneCameraComponent>();

            
            var myPlayerUnitEntity = world.AllEntity.GetValueOrDefault(response.MyPlayerUnitEntity.InsId);
            world.MyPlayer = myPlayerUnitEntity;
            
            unitySceneCameraComponent.SetFlowUnitEntity(world.MyPlayer);
            
            FGUIComponent.Instance.CloseWindowAll();
            FGUIComponent.Instance.ShowWindowAsync(WindowID.FGUIBattleOperationMainView).Coroutine();
        }
    }
}