namespace ET.Client
{
    [ClientWorldEventHandler]
    public class UnitEntityElementUpdate_ClientUpdateElementData: AClientWorldEvent<ClientUpdateElementData>
    {
        protected override async ETTask Run(ClientWorld world, ClientUpdateElementData args)
        {
            var unitEntity = args.UnitEntity;
            var componentId = args.ComponentId;

            var logics = ClientWorldLogicManagerComponent.Instance.GetUpdateLogic(componentId);
            if (logics != null)
            {
                foreach (var logic in logics)
                {
                    logic.OnUpdate(unitEntity, args.OldUnitEntityElemData, args.NewUnitEntityElemData);
                }
            }
        }
    }
}

