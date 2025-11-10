using System;
using System.Collections.Generic;
using System.IO;

namespace ET.Client
{
    [Event(SceneType.Main)]
    public class EntryEvent3_InitClient: AEvent<Scene, EntryEvent3>
    {
        protected override async ETTask Run(Scene root, EntryEvent3 args)
        {
            GlobalComponent globalComponent = root.AddComponent<GlobalComponent>();
            root.AddComponent<PlayerComponent>();
            root.AddComponent<ClientLobbyDataComponent>(); // 客户端 - Lobby 数据同步组件
            
            
            //// 战斗
            // 战斗数据逻辑管理器
            root.AddComponent<BattleUnitEntityLogicManagerComponent>();
            // 战斗显示逻辑
            // root.AddComponent<GizmoDebugComponent>();
            // 战斗逻辑模块
            root.AddComponent<BattleEventManagerComponent>();
            
            
            
            // 客户端战斗世界管理器
            root.AddComponent<ClientWorldEventManagerComponent>();
            root.AddComponent<ClientWorldLogicManagerComponent>();
            
            // 数据加载
            root.AddComponent<ResourcesLoaderComponent>();
            // 场景管理
            root.AddComponent<UnitySceneManagerComponent>();
            
            //// FGUI 
            // FGUI 包管理器组件
            root.AddComponent<FGUIPackageComponent>();
            // FGUI 事件
            root.AddComponent<FGUIManagerComponent>();
            // FGUI UI组件
            root.AddComponent<FGUIComponent>();
            
            // 根据配置修改掉Main Fiber的SceneType
            SceneType sceneType = EnumHelper.FromString<SceneType>(globalComponent.GlobalConfig.AppType.ToString());
            root.SceneType = sceneType;
            
            await EventSystem.Instance.PublishAsync(root, new AppStartInitFinish());
        }
    }
}