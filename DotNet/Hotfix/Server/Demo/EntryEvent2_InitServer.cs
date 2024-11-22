using System;
using System.Net;

namespace ET.Server
{
    [Event(SceneType.Main)]
    public class EntryEvent2_InitServer: AEvent<Scene, EntryEvent2>
    {
        protected override async ETTask Run(Scene root, EntryEvent2 args)
        {
            switch (Options.Instance.AppType)
            {
                case AppType.Server:
                {
                    if (ProcessConfig.Instance.GlobalConfig.InnerPort != 0)
                    {
                        await FiberManager.Instance.Create(SchedulerType.ThreadPool, ConstFiberId.NetInner, 0, SceneType.NetInner, "NetInner");
                    }

                    // 根据配置创建纤程
                    foreach (var sceneConfig in ProcessConfig.Instance.SceneConfigs.Values)
                    {
                        var sceneId = sceneConfig.SceneId;
                        var sceneType = sceneConfig.SceneType;
                        await FiberManager.Instance.Create(SchedulerType.ThreadPool, sceneId, 0, sceneType, $"{sceneType}-{sceneId}");
                    }
                    break;
                }
                case AppType.Watcher:
                {
                    root.AddComponent<WatcherComponent>();
                    break;
                }
                case AppType.GameTool:
                {
                    break;
                }
            }

            if (Options.Instance.Console == 1)
            {
                root.AddComponent<ConsoleComponent>();
            }
        }
    }
}