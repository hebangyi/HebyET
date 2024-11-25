using System.Collections.Generic;

namespace ET.Server;

public static class EtcdHelper
{
    public const string m_prefix = "GA";

    public static string GetRegPath(SceneType type, long sceneId)
    {
        return $"/{m_prefix}/{type}/{sceneId}";
    }

    public static string GetSubPatch( SceneType type)
    {
        return $"/{m_prefix}/{type}/";
    }

    public static EtcdSceneNodeInfo BuildSelfSceneNode(Scene scene, int outPort = 0)
    {
        int processId = Options.Instance.Process;
        string innerIp = ProcessConfig.Instance.GlobalConfig.Ip;
        int innerPort = ProcessConfig.Instance.GlobalConfig.InnerPort;

        EtcdSceneNodeInfo sceneNode = new EtcdSceneNodeInfo();
        sceneNode.SceneType = scene.SceneType;
        sceneNode.ProcessId = processId;
        sceneNode.SceneId = (int)scene.Id;
        sceneNode.InnerIp = innerIp;
        sceneNode.InnerPort = innerPort;
        // TODO 根据不同的服务器 数据相关
        sceneNode.Status = 0;
        return sceneNode;
    }


    public static EtcdSceneNodeInfo GetRandomNode(SceneType sceneType)
    {
        List<EtcdSceneNodeInfo> sceneNodes = EtcdManager.Instance.WatchSceneNodes.GetValueOrDefault(sceneType);
        if (sceneNodes == null || sceneNodes.Count == 0)
        {
            return null;
        }

        if (sceneNodes.Count == 1)
        {
            return sceneNodes[0];
        }

        return sceneNodes.RandomArray();
    }
    
    
    
}