namespace ET.Server;
using dotnet_etcd;

[SingletonConfigOf("Etcd")]
public class EtcdManagerConfig
{
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

public class EtcdManager : Singleton<EtcdManager>, ISingletonAwake
{
    public EtcdManagerConfig Config;
    
    public EtcdClient RegClient;
    public EtcdClient WatchClient;
    

    public Dictionary<SceneType, List<EtcdSceneNodeInfo>> WatchSceneNodes = new();

    //Etcd 监听的 ScenePath
    public HashSet<SceneType> WatchingSceneTypes = new();
    public HashSet<string> WatchingScenePaths = new();

    public void Awake()
    {
        this.Config = ProcessConfig.Instance.GetSingletonConfig<EtcdManagerConfig>();
    }

    /// <summary>
    /// 开始注册与订阅
    /// </summary>
    public void StartRegAndSub()
    {
        Array watchingSceneTypes = Enum.GetValues(typeof(SceneType));
        foreach (SceneType watchingSceneType in watchingSceneTypes)
        {
            Instance.WatchingSceneTypes.Add(watchingSceneType);
        }

        foreach (SceneType sceneType in Instance.WatchingSceneTypes)
        {
            var path = EtcdHelper.GetSubPatch(sceneType);
            Instance.WatchingScenePaths.Add(path);
        }
    }

    /// <summary>
    /// 开始订阅
    /// </summary>
    public async ETTask StartSub()
    {
        
        
        
        
        await ETTask.CompletedTask;
    }
}