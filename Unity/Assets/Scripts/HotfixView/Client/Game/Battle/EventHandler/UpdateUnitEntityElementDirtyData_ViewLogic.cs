namespace ET.Client
{
    [ClientWorldEventHandler]
    public class UpdateUnitEntityElementDirtyData_ViewLogic: AClientWorldEvent<ClientUpdateElementData>
    {
        protected override void Run(ClientWorld world, ClientUpdateElementData args)
        {
            var unitEntity = args.UnitEntity;
            var componentId = args.ComponentId;

            var logics = ClientWorldLogicManagerComponent.Instance.GetUpdateLogicByComponentId(componentId);
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

