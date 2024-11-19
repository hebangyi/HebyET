using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace ET.Server
{
    
    [ComponentOf(typeof(Scene))]
    public class RouterComponent: Entity, IAwake, IDestroy, IUpdate
    {
        public RouterComponentConfig Config;
        
        public IKcpTransport OuterUdp;
        public IKcpTransport OuterTcp;
        public IKcpTransport InnerSocket;
        public EndPoint IPEndPoint = new IPEndPoint(IPAddress.Any, 0);

        public byte[] Cache = new byte[1500];

        public Queue<uint> checkTimeout = new();

        public long LastCheckTime = 0;
    }
    
    [ComponentConfigOf("RouterComponent")]
    public class RouterComponentConfig
    {
        public string OuterAddress = "127.0.0.1:30300";
        public int OuterPort = 30300;
        public string InnerIp = "127.0.0.1";
    }
}