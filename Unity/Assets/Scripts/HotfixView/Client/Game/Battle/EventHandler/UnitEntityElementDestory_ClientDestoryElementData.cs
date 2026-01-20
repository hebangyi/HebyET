namespace ET.Client
{
    [ClientWorldEventHandler]
    public class UnitEntityElementDestory_ClientDestoryElementData: AClientWorldEvent<ClientDestroyElementData>
    {
        protected override async ETTask Run(ClientWorld world, ClientDestroyElementData args)
        {
            var unitEntity = args.UnitEntity;
            var componentId = args.ComponentId;
            
            var logics = ClientWorldLogicManagerComponent.Instance.GetInitViewLogic(componentId);
            if (logics != null)
            {
                foreach (var logic in logics)
                {
                    logic.OnDestroy(unitEntity, args.UnitEntityElemData);
                }
            }
        }
    }
}