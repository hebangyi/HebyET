using System.Collections.Generic;

namespace ET.Server;


[ComponentOf(typeof(Scene))]
public class RouterServerComponent : Entity, IAwake
{
    public Dictionary<SceneType, ConsistentHash<SceneNodeInfo>> SceneType2NodeInfos = new();
}