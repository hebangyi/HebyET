namespace ET.Client
{
    [ClientWorldEventHandler]
    public class ClientCameraPositionUpdate_OrderUnitEntityLayer: AClientWorldEvent<ClientCameraPositionUpdate>
    {
        protected override async ETTask Run(ClientWorld world, ClientCameraPositionUpdate args)
        {
            var allEntities = world.AllEntities;
            foreach (var allEntity in allEntities.Values)
            {
                allEntity.UpdateOrderLayer();
            }
            await ETTask.CompletedTask;
        }
    }
}