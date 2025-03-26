namespace ET.Client
{
    [MessageHandler(SceneType.Game)]
    public class L2C_SyncDirtyDataUnitsHandler : MessageHandler<Scene, L2C_SyncDirtyDataUnits>
    {
        protected override async ETTask Run(Scene scene, L2C_SyncDirtyDataUnits message)
        {
            // 同步脏数据
            // TODO dirty Frame 有错误
            ClientLobbyDataComponentHelper.SyncDirtyData(scene, message.UnitStructData);
            await ETTask.CompletedTask;
        }
    }
}