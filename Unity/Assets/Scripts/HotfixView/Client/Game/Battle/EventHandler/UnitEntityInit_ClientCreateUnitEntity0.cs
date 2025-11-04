namespace ET.Client
{
    [ClientWorldEventHandler]
    public class UnitEntityInit_ClientCreateUnitEntity0: AClientWorldEvent<ClientCreateUnitEntity0>
    {
        protected override async ETTask Run(ClientWorld world, ClientCreateUnitEntity0 args)
        {
            var unitEntity = args.UnitEntity;
            var unitEntityCommonData = unitEntity.GetUnitEntityElemData<UnitEntityCommonData>();
            var unitEntityContext = ClientWorldLogicManagerComponent.Instance.GetClientUnitEntityContext(unitEntityCommonData.UnitEntityType);
            if (unitEntityContext == null)
            {
                unitEntityContext = ClientWorldLogicManagerComponent.Instance.GetClientUnitEntityContext(UETypeEnum.None);
            }
            
            unitEntityContext.Init(unitEntity);
            unitEntityContext.CreateView(unitEntity);
        }
    }
}