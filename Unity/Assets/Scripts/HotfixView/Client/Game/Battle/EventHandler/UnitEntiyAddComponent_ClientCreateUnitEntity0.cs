namespace ET.Client;

[ClientWorldEventHandler]
public class UnitEntiyAddComponent_ClientCreateUnitEntity0: AClientWorldEvent<ClientUnitEntityGameObject>
{
    protected override async ETTask Run(ClientWorld world, ClientUnitEntityGameObject args)
    {
        var unitEntity = args.UnitEntity;
        
    }
}