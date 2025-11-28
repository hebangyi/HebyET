using NLog;

namespace ET.Client
{
    [UnitySceneContext(UnitySceneEnum.Battle)]
    public class BattleUnityScene : BaseUnityScene
    {
        public override async ETTask LoadingCompleted(UnityScene unityScene)
        {
            var clientBattleSenderComponent = unityScene.Root().GetComponent<ClientBattleSenderComponent>();
            C2B_PlayerGetAllAOIWorldData request = C2B_PlayerGetAllAOIWorldData.Create();
            B2C_PlayerGetAllAOIWorldData response = (B2C_PlayerGetAllAOIWorldData)await clientBattleSenderComponent.Call(request);
            
            if (response.Error != ErrorCode.ERR_Success)
            {
                SceneChangeHelper.SceneChangeTo(unityScene.Root(), UnitySceneEnum.Lobby).Coroutine();
                return;
            }
            
            ClientWorld clientWorld = UnitySceneClientWorldManagerComponent.Instance.CreateWorld();
            clientWorld.MainPlayerId = response.MyPlayerUnitEntity.InsId;
            
            // 加载资源
            await clientWorld.LoadUnityObject();
            await clientWorld.InitWorld(response.BattleWorld);
            await clientWorld.AddBattleUnit(response.MyPlayerUnitEntity);
            await clientWorld.AddBattleUnits(response.AOIBattleUnitEntity);
            await clientWorld.AddBattleUnits(response.BattleFieldUnitEntity);
            

            FGUIComponent.Instance.CloseWindowAll();
            FGUIComponent.Instance.ShowWindowAsync(WindowID.FGUIBattleOperationMainView).Coroutine();
            
            await ETTask.CompletedTask;
        }
    }
}

