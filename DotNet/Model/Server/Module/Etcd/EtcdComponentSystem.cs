﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Etcdserverpb;
using Google.Protobuf;
using dotnet_etcd;
using Grpc.Core;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MongoDB.Bson;

namespace ET.Server;

[EntitySystemOf(typeof(EtcdComponent))]
[FriendOf(typeof(EtcdComponent))]
[FriendOf(typeof(NetComponent))]
public static partial class EtcdComponentSystem
{
    [EntitySystem]
    private static void Awake(this EtcdComponent self)
    {
        // 显式指定 GrpcChannelOptions

        self.Config = ProcessConfig.Instance.GetSceneComponentConfig<EtcdComponentConfig>(self.Root());
        self.RegClient = new EtcdClient(self.Config.EtcdAddress,
            configureChannelOptions: options => { options.Credentials = ChannelCredentials.Insecure; });
        self.EtcdWatchClient = new EtcdClient(self.Config.EtcdAddress,
            configureChannelOptions: options => { options.Credentials = ChannelCredentials.Insecure; });
    }

    /// <summary>
    /// 开始注册与订阅
    /// </summary>
    public static void StartRegAndWatch(this EtcdComponent self)
    {
        self.CalEtcdTarget();
        self.StartReg().Coroutine();
        self.StartWatch().Coroutine();
    }

    /// <summary>
    /// 计算ETCD 注册和监听的目标
    /// </summary>
    public static void CalEtcdTarget(this EtcdComponent self)
    {
        Array watchingSceneTypes = Enum.GetValues(typeof(SceneType));
        foreach (SceneType watchingSceneType in watchingSceneTypes)
        {
            EtcdManager.Instance.WatchingSceneTypes.Add(watchingSceneType);
        }

        foreach (SceneType sceneType in EtcdManager.Instance.WatchingSceneTypes)
        {
            var path = EtcdHelper.GetSubPatch(sceneType);
            EtcdManager.Instance.WatchingScenePaths.Add(path);
        }

        var fiberInner = FiberManager.Instance.fibers.Values.FirstOrDefault(x => x.Root.SceneType == SceneType.NetInner);
        if (fiberInner != null)
        {
            foreach (var fiber in FiberManager.Instance.fibers.Values)
            {
                var scene = fiber.Root;
                var etcdClientComponent = scene.GetComponent<EtcdClientComponent>();
                if (etcdClientComponent == null)
                {
                    continue;
                }

                EtcdSceneNodeInfo etcdSceneNodeInfo = null;
                if (scene.SceneType is SceneType.Gate or SceneType.Account)
                {
                    var netComponent = scene.GetComponent<NetComponent>();
                    int outPort = 0;
                    if (netComponent != null)
                    {
                        outPort = netComponent.OutPort;
                    }

                    etcdSceneNodeInfo = EtcdHelper.BuildSelfSceneNode(scene, outPort);
                }
                else
                {
                    etcdSceneNodeInfo = EtcdHelper.BuildSelfSceneNode(scene, 0);
                }

                EtcdClient client = self.RegClient;
                var sceneId = scene.Id;
                // var lease = await client.LeaseGrantAsync(new LeaseGrantRequest { TTL = 90 });
                // var leaseId = lease.ID;
                var regPath = ByteString.CopyFromUtf8(EtcdHelper.GetRegPath(scene.SceneType, scene.Id));
                var regValue = ByteString.CopyFromUtf8(JsonHelper.ToJson(etcdSceneNodeInfo));

                RegSceneNodePack regSceneNodePack = new();
                regSceneNodePack.SceneId = sceneId;
                regSceneNodePack.RegPath = regPath;
                regSceneNodePack.RegValue = regValue;
                regSceneNodePack.SceneNodeInfo = etcdSceneNodeInfo;
                EtcdManager.Instance.RegSceneNodePacks.Add(regSceneNodePack);
            }
        }
        else
        {
            Log.Error("没有找到 fiber NetInner 内网组件 注册Etcd失败");
        }
    }

    /// <summary>
    /// 开始注册
    /// </summary>
    private static async ETTask StartReg(this EtcdComponent self)
    {
        await self.RegEtcd();
        // var timerComponent = self.Root().GetComponent<TimerComponent>();
        // await timerComponent.WaitAsync(60 * 1000);
        // await self.StartReg();
    }

    public static async ETTask RegEtcd(this EtcdComponent self)
    {
        EtcdClient client = self.RegClient;
        foreach (var pack in EtcdManager.Instance.RegSceneNodePacks)
        {
            await self.RegEtcdNode(pack);
        }
        self.Fiber().ThreadSynchronizationContext.Post(() =>
        {
            self.StartKeepAliveTimer().Coroutine();    
        });
    }


    public static async ETTask RegEtcdNode(this EtcdComponent self, RegSceneNodePack pack)
    {
        EtcdClient client = self.RegClient;
        var lease = await client.LeaseGrantAsync(new LeaseGrantRequest { TTL = self.Config.TTL }, cancellationToken: ServerManager.Instance.CancellationToken.Token);
        var leaseId = lease.ID;
        var request = new PutRequest();
        request.Lease = leaseId;
        request.Key = pack.RegPath;
        request.Value = pack.RegValue;
        var response = await client.PutAsync(request, cancellationToken: ServerManager.Instance.CancellationToken.Token);
        pack.LeaseId = leaseId;
    }


    public static async ETTask StartKeepAliveTimer(this EtcdComponent self)
    {
        EtcdClient client = self.RegClient;
        foreach (var pack in EtcdManager.Instance.RegSceneNodePacks)
        {
            var leaseId = pack.LeaseId;
            if (leaseId == 0)
            {
                Log.Info($"ETCD 重新注册 {pack.RegPath.ToString()}");
                await self.RegEtcdNode(pack);
            }
            
            LeaseKeepAliveRequest keepAliveRequest = new LeaseKeepAliveRequest();
            keepAliveRequest.ID = leaseId;
            // 开启续约
            await client.LeaseKeepAlive(keepAliveRequest, (res) =>
            {
                // 表明已经失效 需要重新注册 
                if (res.TTL == 0)
                {
                    Log.Info("ETCD TTL失效 重新注册");
                    pack.LeaseId = 0;
                }
            }, ServerManager.Instance.CancellationToken.Token);
        }
        var timerComponent = self.Root().GetComponent<TimerComponent>();
        await timerComponent.WaitAsync(self.Config.Interval * 1000);
        // 不循环 await调用
        self.Fiber().ThreadSynchronizationContext.Post(() =>
        {
            self.StartKeepAliveTimer().Coroutine();    
        });
    }
    

    /// <summary>
    /// 开始订阅
    /// </summary>
    private static async ETTask StartWatch(this EtcdComponent self)
    {
        await self.EtcdWatchClient.WatchRangeAsync(EtcdManager.Instance.WatchingScenePaths.ToArray(), delegate(WatchEvent[] events)
        {
            self.Fiber().ThreadSynchronizationContext.Post(() =>
            {
                foreach (var data in events)
                {
                    if (data.Type == Mvccpb.Event.Types.EventType.Put)
                    {
                        var node = JsonHelper.FromJson<EtcdSceneNodeInfo>(data.Value);
                        if (node == null) continue;
                        self.OnWatchEvent(data.Key, node, true);
                    }
                    else
                    {
                        self.OnWatchEvent(data.Key, null, false);
                    }
                }
            });
        });

        self.StartWatch().Coroutine();
    }

    public static void OnWatchEvent(this EtcdComponent self, string key, EtcdSceneNodeInfo sceneNode, bool isCreate)
    {
        string[] pathVal = key.Split("/");
        SceneType sceneType = Enum.Parse<SceneType>(pathVal[^2]);
        long sceneId = long.Parse(pathVal.Last());

        if (isCreate)
        {
            // remove
            if (EtcdManager.Instance.WatchSceneNodes.TryGetValue(sceneType, out var list))
            {
                EtcdSceneNodeInfo removeNode = list.Find(x => x.SceneId == sceneId);
                if (removeNode != null)
                {
                    list.Remove(removeNode);
                }
            }

            EtcdManager.Instance.WatchId2SceneNodes.Remove(sceneId, out _);
            // add
            if (!EtcdManager.Instance.WatchSceneNodes.TryGetValue(sceneType, out list))
            {
                list = new();
                EtcdManager.Instance.WatchSceneNodes[sceneType] = list;
            }

            list.Add(sceneNode);
            EtcdManager.Instance.WatchId2SceneNodes[sceneId] = sceneNode;
            Log.Info($"ETCD WatchEvent : Create Node :\n {JsonHelper.ToJson(sceneNode)}");
        }
        else
        {
            if (EtcdManager.Instance.WatchSceneNodes.TryGetValue(sceneType, out var list))
            {
                EtcdSceneNodeInfo removeNode = list.Find(x => x.SceneId == sceneId);
                if (removeNode != null)
                {
                    list.Remove(removeNode);
                }
            }

            EtcdManager.Instance.WatchId2SceneNodes.Remove(sceneId, out _);
            Log.Info($"ETCD WatchEvent RemoveNode , SceneType : {sceneType}, Scene sceneId : {sceneId}");
        }
    }
}