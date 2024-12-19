using System.Collections.Generic;

namespace ET.Server;


[EntitySystemOf(typeof(RouterServerComponent))]
[FriendOf(typeof(RouterServerComponent))]
public static partial class RouterServerComponentSystem
{
    [EntitySystem]
    private static void Awake(this RouterServerComponent self)
    {
        
    }

    public static void AddServerNode(this RouterServerComponent self, SceneNodeInfo sceneNodeInfo)
    {
        var consistentHash = self.GetOrCreateConsistentHash((SceneType)sceneNodeInfo.SceneType);
        consistentHash.Add(sceneNodeInfo.SceneId.ToString(), sceneNodeInfo);
    }

    public static void RemoveServerNode(this RouterServerComponent self, SceneNodeInfo sceneNodeInfo)
    {
        var consistentHash = self.GetOrCreateConsistentHash((SceneType)sceneNodeInfo.SceneType);
        consistentHash.Remove(sceneNodeInfo.SceneId.ToString());
    }

    private static ConsistentHash<SceneNodeInfo> GetOrCreateConsistentHash(this RouterServerComponent self, SceneType sceneType)
    {
        var consistentHash = self.SceneType2NodeInfos.GetValueOrDefault(sceneType);
        if (consistentHash == null)
        {
            consistentHash = new ConsistentHash<SceneNodeInfo>();
        }

        return consistentHash;
    }

    public static SceneNodeInfo GetRouterServerNode(this RouterServerComponent self, SceneType sceneType, string key)
    {
        var consistentHash = self.GetOrCreateConsistentHash(sceneType);
        SceneNodeInfo node = consistentHash.Get(key);
        return node;
    }
}