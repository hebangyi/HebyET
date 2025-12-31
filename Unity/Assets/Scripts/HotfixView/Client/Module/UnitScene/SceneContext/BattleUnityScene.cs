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
            clientWorld.CacheDirtyMessage.Clear();
            clientWorld.ClientWorldStatusEnum = ClientWorldStatusEnum.InitData;
            await clientWorld.InitWorld(response.BattleWorld);
            
            clientWorld.MainPlayerId = response.MyPlayerUnitEntity.InsId;
            
            // 加载资源
            await clientWorld.LoadUnityObject();
            
            clientWorld.AddBattleUnit(response.MyPlayerUnitEntity);
            clientWorld.AddBattleUnits(response.AOIBattleUnitEntity);
            clientWorld.AddBattleUnits(response.BattleFieldUnitEntity);
            
            // 处理缓存数据

            for (int i = 0; i < clientWorld.CacheDirtyMessage.Count; i++)
            {
                var dirtyMessage = clientWorld.CacheDirtyMessage[i];
                if (clientWorld.Frame != dirtyMessage.LastSyncFrame)
                {
                    // TODO
                    Log.Error("初始化世界处理缓存推送数据帧数异常 重新初始化!");
                    return;
                }
                
                clientWorld.HandleDirtyMessage(dirtyMessage);
            }

            clientWorld.ClientWorldStatusEnum = ClientWorldStatusEnum.Run;
            
            FGUIComponent.Instance.CloseWindowAll();
            FGUIComponent.Instance.ShowWindowAsync(WindowID.FGUIBattleOperationMainView).Coroutine();
            
            await ETTask.CompletedTask;
        }
    }
}

