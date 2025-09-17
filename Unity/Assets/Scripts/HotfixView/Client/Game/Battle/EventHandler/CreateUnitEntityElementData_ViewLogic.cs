namespace ET.Client
{
    [ClientWorldEventHandler]
    public class CreateUnitEntityElementData_ViewLogic: AClientWorldEvent<CreateUnitEntityElementData>
    {
        protected override void Run(ClientWorld world, CreateUnitEntityElementData args)
        {
            var unitEntity = args.UnitEntity;
            var componentId = args.ComponentId;
            
            var logics = ClientWorldLogicManagerComponent.Instance.GetInitViewLogicByComponentId(componentId);
            if (logics != null)
            {
                foreach (var logic in logics)
                {
                    logic.OnInit(unitEntity);
                }
            }
        }
    }
}