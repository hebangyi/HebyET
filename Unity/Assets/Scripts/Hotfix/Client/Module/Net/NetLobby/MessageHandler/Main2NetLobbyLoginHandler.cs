using System;
using System.Net;
using System.Net.Sockets;

namespace ET.Client
{
    [MessageHandler(SceneType.NetLobby)]
    public class Main2NetLobbyLoginHandler: MessageHandler<Scene, Main2NetLobbyLogin, NetLobby2MainLogin>
    {
        protected override async ETTask Run(Scene root, Main2NetLobbyLogin request, NetLobby2MainLogin response)
        {
            string account = request.Account;
            string password = request.Password;
            // 创建一个ETModel层的Session
            root.RemoveComponent<RouterAddressComponent>();
            
            // TODO 更具环境 获取 服务器Http地址
            
            // 获取路由跟realmDispatcher地址
            RouterAddressComponent routerAddressComponent =
                    root.AddComponent<RouterAddressComponent, string, int>(GameConstant.EntryServerHttpHost, GameConstant.EntryServerHttpPort);
            await routerAddressComponent.Init();
            
            IPEndPoint routerAddress = routerAddressComponent.GetAddress();
            var netComponent = root.AddComponent<NetComponent, AddressFamily, NetworkProtocol>(routerAddress.AddressFamily, NetworkProtocol.UDP);
            root.GetComponent<FiberParentComponent>().ParentFiberId = request.OwnerFiberId;
            
            IPEndPoint realmAddress = routerAddressComponent.GetRealmAddress(account);
            
            // TODO 链接异常处理
            A2C_Login a2CLogin;
            using (Session session = await netComponent.CreateRouterSession(routerAddress, realmAddress))
            {
                C2A_Login c2ALogin = C2A_Login.Create();
                c2ALogin.Account = account;
                c2ALogin.Password = password;
                a2CLogin = (A2C_Login)await session.Call(c2ALogin);
            }
 
            if (a2CLogin.Error != (int)ErrorCode.ERR_Success)
            {
                response.Error = a2CLogin.Error;
                return;
            }
            
            // 创建一个gate Session,并且保存到SessionComponent中
            Session gateSession = await netComponent.CreateRouterSession(routerAddress, NetworkHelper.ToIPEndPoint(a2CLogin.Address));
            gateSession.AddComponent<ClientSessionErrorComponent>();
            root.AddComponent<SessionComponent>().Session = gateSession;
            
            C2L_LoginLobby c2GLoginLobby = C2L_LoginLobby.Create();
            c2GLoginLobby.Token = a2CLogin.Token;
            L2C_LoginLobby g2CLoginGate = (L2C_LoginLobby)await gateSession.Call(c2GLoginLobby);
            response.PlayerId = g2CLoginGate.PlayerId;
        }
    }
}