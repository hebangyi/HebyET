using System.Collections.Concurrent;

namespace ET.Server;

public class EtcdManager : Singleton<EtcdManager>, ISingletonAwake
{
    // 本服注册的SceneNodes
    public ConcurrentDictionary<string, EtcdSceneNodeInfo> RegSceneNodes = new();

    // 本服监听的SceneNodes
    public ConcurrentDictionary<SceneType, List<EtcdSceneNodeInfo>> WatchSceneNodes = new();
    public ConcurrentDictionary<long, EtcdSceneNodeInfo> WatchId2SceneNodes = new();

    //监听的 SceneTypes
    public HashSet<SceneType> WatchingSceneTypes = new();
    //监听的 ScenePath
    public HashSet<string> WatchingScenePaths = new();

    public void Awake()
    {
        Array watchingSceneTypes = Enum.GetValues(typeof(SceneType));
        foreach (SceneType watchingSceneType in watchingSceneTypes)
        {
            this.WatchingSceneTypes.Add(watchingSceneType);
        }

        foreach (SceneType sceneType in this.WatchingSceneTypes)
        {
            var path = EtcdHelper.GetSubPatch(sceneType);
            this.WatchingScenePaths.Add(path);
        }
    }
}