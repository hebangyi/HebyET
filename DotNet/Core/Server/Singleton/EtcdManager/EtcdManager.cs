using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ET.Server;


public class EtcdSceneNodeInfo
{
    public SceneType SceneType;
    public int ProcessId;
    public int SceneId;
    public string OuterIp;
    public string InnerIp;
    public int InnerPort;
    public int OuterPort;
    public int Status;

    public string InnerIpAndOuterPortAddress
    {
        get { return $"{this.InnerIp}:{this.OuterPort}"; }
    }

    public string InnerIpAndInnerPortAddress
    {
        get { return $"{this.InnerIp}:{this.InnerPort}"; }
    }

    public string OuterIpAndOuterPortAddress
    {
        get { return $"{this.OuterIp}:{this.OuterPort}"; }
    }
}

public class RegSceneNodePack
{
    // 续约ID
    public long LeaseId;
    // 场景ID
    public long SceneId;
    // 注册的地址
    public Google.Protobuf.ByteString RegPath;
    // 注册的信息
    public Google.Protobuf.ByteString RegValue;
    // 节点信息
    public EtcdSceneNodeInfo SceneNodeInfo;
    
}


public class EtcdManager : Singleton<EtcdManager>, ISingletonAwake
{
    // 本服注册的Etcd信息
    public Dictionary<int, RegSceneNodePack> SceneId2RegSceneNodePacks = new();
    
    // 本服监听的SceneNodes
    public ConcurrentDictionary<SceneType, List<EtcdSceneNodeInfo>> WatchSceneNodes = new();
    public ConcurrentDictionary<long, EtcdSceneNodeInfo> WatchId2SceneNodes = new();

    //监听的 SceneTypes
    public HashSet<SceneType> WatchingSceneTypes = new();
    //监听的 ScenePath
    public HashSet<string> WatchingScenePaths = new();
    public void Awake()
    {
    }
}