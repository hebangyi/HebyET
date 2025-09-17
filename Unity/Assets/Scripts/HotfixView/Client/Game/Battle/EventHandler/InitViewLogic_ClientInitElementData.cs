namespace ET.Client
{
    [ClientWorldEventHandler]
    public class InitViewLogic_ClientInitElementData: AClientWorldEvent<ClientInitElementData>
    {
        protected override async ETTask Run(ClientWorld world, ClientInitElementData args)
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