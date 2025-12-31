namespace ET.Client
{
    [MessageHandler(SceneType.Game)]
    public class L2C_PlayerAOIWorldDirtyPushHandler : MessageHandler<Scene, L2C_PlayerAOIWorldDirtyPush>
    {
        protected override async ETTask Run(Scene entity, L2C_PlayerAOIWorldDirtyPush message)
        {
            ClientWorld world = UnitySceneClientWorldManagerComponent.Instance.CurrentClientWorld;
            if (world == null)
            {
                Log.Error($"收到 DirtyPush数据 没有找到客户端世界数据 : {message.CurrentSyncFrame}");
                return;
            }

            if (world.ClientWorldStatusEnum == ClientWorldStatusEnum.None)
            {
                return;
            }
            
            if (world.ClientWorldStatusEnum == ClientWorldStatusEnum.InitData)
            {
                world.CacheDirtyMessage.Add(message);
                return;
            }
            else if(world.ClientWorldStatusEnum == ClientWorldStatusEnum.Run)
            {
                if (world.Frame != message.LastSyncFrame)
                {
                    // TODO 重连
                    Log.Error($"world {world.Id} L2C_PlayerAOIWorldDirtyPush 推送脏数据信息 Frame 有误 , 重新请求信息 {world.Frame}, {message.LastSyncFrame}");
                    return;
                }
            }
            
            // 正常更新
            world.HandleDirtyMessage(message);
            
            await ETTask.CompletedTask;
        }
    }
}
