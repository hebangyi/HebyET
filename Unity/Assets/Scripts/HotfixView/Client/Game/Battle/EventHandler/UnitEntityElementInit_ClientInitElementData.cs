namespace ET.Client
{
    [ClientWorldEventHandler]
    public class UnitEntityElementInit_ClientInitElementData: AClientWorldEvent<ClientInitElementData>
    {
        protected override async ETTask Run(ClientWorld world, ClientInitElementData args)
        {
            var unitEntity = args.UnitEntity;
            var componentId = args.ComponentId;
            
            var logics = ClientWorldLogicManagerComponent.Instance.GetInitViewLogic(componentId);
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