using System;
using System.Net;
using System.Net.Sockets;


namespace ET.Client
{
    [MessageHandler(SceneType.NetBattle)]
    public class Main2NetBattleLoginHandler: MessageHandler<Scene, Main2NetBattleLogin, NetBattle2MainLogin>
    {
        protected override async ETTask Run(Scene root, Main2NetBattleLogin request, NetBattle2MainLogin response)
        {
            var token = request.Token;
            string routerAddress = request.RouterAddress;
            string address = request.Address;


            var routerAddressIPEndPoint = NetworkHelper.ToIPEndPoint(routerAddress);
            var addressIPEndPoint = NetworkHelper.ToIPEndPoint(address);
            
            var netComponent = root.AddComponent<NetComponent, AddressFamily, NetworkProtocol>(routerAddressIPEndPoint.AddressFamily, NetworkProtocol.UDP);
            root.GetComponent<FiberParentComponent>().ParentFiberId = request.OwnerFiberId;
            
            Session gateSession = await netComponent.CreateRouterSession(routerAddressIPEndPoint, addressIPEndPoint);
            gateSession.AddComponent<ClientSessionErrorComponent>();
            root.AddComponent<SessionComponent>().Session = gateSession;

            C2B_Login c2bLogin = C2B_Login.Create();
            c2bLogin.Token = token;
            B2C_Login b2CLogin = (B2C_Login)await gateSession.Call(c2bLogin);
            response.Error = b2CLogin.Error;
            await ETTask.CompletedTask;
        }
    }
}