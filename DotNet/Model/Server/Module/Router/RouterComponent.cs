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

        public int OuterPort;
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
        public int OuterPort = 30300;
    }
}