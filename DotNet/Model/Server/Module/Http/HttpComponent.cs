using System.Collections.Generic;
using System.Net;

namespace ET.Server
{
    /// <summary>
    /// http请求分发器
    /// </summary>
    [ComponentOf(typeof(Scene))]
    public class HttpComponent: Entity, IAwake ,IAwake<string>, IDestroy
    {
        public HttpListener Listener;
        public HttpComponentConfig Config;
    }

    [ComponentConfigOf("HttpComponent")]
    public class HttpComponentConfig
    {
        public string Addresses = "http://*:8080";
    }
}