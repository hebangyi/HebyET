using System.Collections.Concurrent;
using System.Collections.Generic;


namespace ET;


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
    public SceneNodeInfo SceneNodeInfo;
    
}


[Code]
public class EtcdManager : Singleton<EtcdManager>, ISingletonAwake
{
    // 本服注册的Etcd信息
    public Dictionary<int, RegSceneNodePack> SceneId2RegSceneNodePacks = new();
    
    // 本服监听的SceneNodes
    public ConcurrentDictionary<SceneType, List<SceneNodeInfo>> WatchSceneNodes = new();
    public ConcurrentDictionary<int, SceneNodeInfo> WatchId2SceneNodes = new();

    //监听的 SceneTypes
    public HashSet<SceneType> WatchingSceneTypes = new();
    //监听的 ScenePath
    public HashSet<string> WatchingScenePaths = new();
    public void Awake()
    {
        
    }
}