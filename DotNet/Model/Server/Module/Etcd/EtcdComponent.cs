using System;
using System.Collections.Generic;
using System.Linq;
using Etcdserverpb;
using Google.Protobuf;
using dotnet_etcd;

namespace ET.Server;

[ComponentConfigOf("EtcdComponent")]
public class EtcdComponentConfig
{
    public string EtcdAddress = "http://127.0.0.1:2379";
    public int TTL = 60;                // 注册失效时间
    public int Interval = 10;           // 心跳周期
}

[ComponentOf(typeof(Scene))]
public class EtcdComponent : Entity, IAwake
{
    public EtcdComponentConfig Config;
    public EtcdClient RegClient;
    public EtcdClient EtcdWatchClient;
}