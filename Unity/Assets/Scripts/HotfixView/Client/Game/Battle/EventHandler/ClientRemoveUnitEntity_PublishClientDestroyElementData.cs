namespace ET.Client
{
    [ClientWorldEventHandler]
    public class ClientRemoveUnitEntity_PublishClientDestroyElementData: AClientWorldEvent<ClientRemoveUnitEntity>
    {
        protected override async ETTask Run(ClientWorld world, ClientRemoveUnitEntity args)
        {
            var unitEntity = args.UnitEntity;
            var clientWorld = unitEntity.ClientWorld();
            foreach (var unitEntityElemDataKv in unitEntity.UnitEntityData)
            {
                clientWorld.PublishEvent(new ClientDestroyElementData()
                        { UnitEntity = unitEntity, UnitEntityElemData = unitEntityElemDataKv.Value, ComponentId = unitEntityElemDataKv.Key });
            }

            await ETTask.CompletedTask;
        }
    }
}

