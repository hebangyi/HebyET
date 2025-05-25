using System;
using System.Collections.Generic;
using System.IO;
using ET.Model;

namespace ET.Client
{
    [Event(SceneType.Main)]
    public class EntryEvent3_InitClient: AEvent<Scene, EntryEvent3>
    {
        protected override async ETTask Run(Scene root, EntryEvent3 args)
        {
            GlobalComponent globalComponent = root.AddComponent<GlobalComponent>();
            root.AddComponent<PlayerComponent>();
            root.AddComponent<CurrentScenesComponent>();
            root.AddComponent<ClientLobbyDataComponent>(); // 客户端 - Lobby 数据同步组件
            
            
            // 数据加载
            root.AddComponent<ResourcesLoaderComponent>();
            // 场景管理
            root.AddComponent<UnitySceneManagerComponent>();
            
            //// FGUI 
            // FGUI 包管理器组件
            root.AddComponent<FGUIPackageComponent>();
            // FGUI 事件
            root.AddComponent<FGUIEventComponent>();
            // FGUI UI组件
            root.AddComponent<FGUIComponent>();
            
            // 根据配置修改掉Main Fiber的SceneType
            SceneType sceneType = EnumHelper.FromString<SceneType>(globalComponent.GlobalConfig.AppType.ToString());
            root.SceneType = sceneType;
            
            await EventSystem.Instance.PublishAsync(root, new AppStartInitFinish());
        }
    }
}