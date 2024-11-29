using System;
using System.Collections.Generic;
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
                    root.AddComponent<EtcdComponent>();
                    if (ProcessConfig.Instance.GlobalConfig.InnerPort != 0)
                    {
                        await FiberManager.Instance.Create(SchedulerType.ThreadPool, ConstFiberId.NetInner, 0, SceneType.NetInner, "NetInner");
                    }

                    // 根据配置创建纤程
                    foreach (var sceneConfig in ProcessConfig.Instance.SceneConfigs.Values)
                    {
                        if (sceneConfig.SceneType == SceneType.Main)
                        {
                            continue;
                        }
                        var sceneId = sceneConfig.SceneId;
                        var sceneType = sceneConfig.SceneType;
                        var fiberId = await FiberManager.Instance.Create(SchedulerType.ThreadPool, sceneId, 0, sceneType, $"{sceneType}-{sceneId}");
                        var fiber = FiberManager.Instance.fibers.GetValueOrDefault(fiberId);
                        if (fiber != null)
                        {
                            // TODO 通用组件
                            // TODO 在自己的纤程中加入组件
                            fiber.Root.AddComponent<EtcdClientComponent>();
                        }
                    }
                    await EventSystem.Instance.PublishAsync(root, new InitServerFinish1());
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