namespace ET.Client
{
    [ClientWorldEventHandler]
    public class LoggerElementInit_ClientInitElementData: AClientWorldEvent<ClientInitElementData>
    {
        protected override async ETTask Run(ClientWorld world, ClientInitElementData args)
        {
            var unitEntity = args.UnitEntity;
            // Log.Info($"UnitEntity : {unitEntity.InsId}; InitElement : {JsonHelper.ToJson(args.UnitEntityElemData)}");
        }
    }
    
    
    [ClientWorldEventHandler]
    public class LoggerElementUpdate_ClientUpdateElementData: AClientWorldEvent<ClientUpdateElementData>
    {
        protected override async ETTask Run(ClientWorld world, ClientUpdateElementData args)
        {
            var unitEntity = args.UnitEntity;
            var componentId = args.ComponentId;

            // Log.Info($"UnitEntity : {unitEntity.InsId}; UpdateElementData : {JsonHelper.ToJson(args.NewUnitEntityElemData)}");
        }
    }
}