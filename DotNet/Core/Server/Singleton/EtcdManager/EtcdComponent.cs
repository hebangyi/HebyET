using Etcdserverpb;
using Google.Protobuf;
using OfficeOpenXml.ConditionalFormatting;

namespace ET.Server;

using dotnet_etcd;

[SingletonConfigOf("EtcdComponent")]
public class EtcdComponentConfig
{
    public string EtcdAddress = "http://127.0.0.1:2379";
}

public class EtcdSceneNodeInfo
{
    public SceneType SceneType;
    public int ProcessId;
    public int SceneId;
    public string InnerIp;
    public int InnerPort;
    public int Status;
}

[ComponentOf(typeof(Scene))]
public class EtcdComponent: Entity, IAwake
{
    public EtcdComponentConfig Config;

    public EtcdClient RegClient;
    public EtcdClient EtcdWatchClient;
    
    
    public void Awake()
    {
        this.Config = ProcessConfig.Instance.GetSceneComponentConfig<EtcdComponentConfig>(this.Root());
    }

    /// <summary>
    /// 开始注册与订阅
    /// </summary>
    public void StartRegAndWatch()
    {
        this.RegClient = new EtcdClient(this.Config.EtcdAddress);
        this.EtcdWatchClient = new EtcdClient(this.Config.EtcdAddress);

        
        StartReg().Coroutine();
        StartWatch().Coroutine();
    }

    /// <summary>
    /// 开始注册
    /// </summary>
    private async ETTask StartReg()
    {
        foreach (var regSceneNode in EtcdManager.Instance.RegSceneNodes.ToDictionary())
        {
            EtcdClient client = this.RegClient;
            var lease = await client.LeaseGrantAsync(new LeaseGrantRequest { TTL = 90 });
            var request = new PutRequest();
            request.Lease = lease.ID;
            request.Key = ByteString.CopyFromUtf8(regSceneNode.Key);
            request.Value = ByteString.CopyFromUtf8(JsonHelper.ToJson(regSceneNode.Value));
            await client.PutAsync(request);
        }
        
        var timerComponent = this.GetComponent<TimerComponent>();
        await timerComponent.WaitAsync(60 * 1000);
        await StartReg();
    }

    /// <summary>
    /// 开始订阅
    /// </summary>
    private async ETTask StartWatch()
    {
        await this.EtcdWatchClient.WatchRangeAsync(EtcdManager.Instance.WatchingScenePaths.ToArray(), delegate(WatchEvent[] events)
        {
            this.Fiber().ThreadSynchronizationContext.Post(() =>
            {
                foreach (var data in events)
                {
                    if (data.Type == Mvccpb.Event.Types.EventType.Put)
                    {
                        var node = JsonHelper.FromJson<EtcdSceneNodeInfo>(data.Value);
                        if (node == null) continue;
                        OnWatchEvent(data.Key, node, true);
                    }
                    else
                    {
                        OnWatchEvent(data.Key, null, false);
                    }
                }
            });
        });

        StartWatch().Coroutine();
    }

    public void OnWatchEvent(string key, EtcdSceneNodeInfo sceneNode, bool isCreate)
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