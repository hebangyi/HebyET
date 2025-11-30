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
                Log.Error("收到 DirtyPush数据 没有找到客户端世界数据");
                return;
            }
            
            await world.AddBattleUnits(message.AddUnitEntiities);
            world.UpdateDirty(message.DirtyUnitEntities);
            world.DeleteEntities(message.DeleteUnitEntites);
            
            await ETTask.CompletedTask;
        }
    }
}
