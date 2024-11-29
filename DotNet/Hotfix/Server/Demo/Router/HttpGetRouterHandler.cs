using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Entry, "/get_router")]
    public class HttpGetRouterHandler : IHttpHandler
    {
        public async ETTask Handle(Scene scene, HttpListenerContext context)
        {
            HttpGetRouterResponse response = HttpGetRouterResponse.Create();
            EtcdSceneNodeInfo accountSceneNode = EtcdHelper.GetRandomNode(SceneType.Account);
            EtcdSceneNodeInfo routerSceneNode = EtcdHelper.GetRandomNode(SceneType.Router);

            if (accountSceneNode != null)
            {
                response.Accounts.Add(accountSceneNode.InnerIpAndOuterPortAddress);
            }

            if (routerSceneNode != null)
            {
                response.Routers.Add(routerSceneNode.OuterIpAndOuterPortAddress);
            }
            
            HttpHelper.Response(context, response);
            await ETTask.CompletedTask;
        }
    }
}