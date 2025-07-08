namespace ET.Client
{
    [MessageHandler(SceneType.Game)]
    public class L2C_PlayerAOIWorldDirtyPushHandler : MessageHandler<Scene, L2C_PlayerAOIWorldDirtyPush>
    {
        protected override async ETTask Run(Scene entity, L2C_PlayerAOIWorldDirtyPush message)
        {
            World world = BattleClientWorldManagerComponent.Instance.CurrentWorld;
            if (world == null)
            {
                Log.Error("收到 DirtyPush数据 没有找到客户端世界数据");
                return;
            }

            world.UpdateDirty(message.DirtyUnitEntities);
            await ETTask.CompletedTask;
        }
    }
}
