using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ET.Server
{
    [HttpHandler(SceneType.Entry, "/get_entry")]
    public class HttpGetEntryHandler : IHttpHandler
    {
        public async ETTask Handle(Scene scene, HttpListenerContext context)
        {
            HttpGetRouterResponse response = HttpGetRouterResponse.Create();
            SceneNodeInfo accountSceneNode = EtcdHelper.GetRandomNode(SceneType.Account);
            SceneNodeInfo routerSceneNode = EtcdHelper.GetRandomNode(SceneType.RouterGate);

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