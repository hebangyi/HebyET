using System;
using UnityEngine;
using YooAsset;

namespace ET.Client
{
    [ChildOf]
    public class UnityScene : Entity, IAwake, IDestroy
    {
        // 场景类型
        public UnitySceneEnum UnitySceneEnum;
        public object[] ParamList;

        // 正在加载场景的句柄
        public SceneHandle LoadingSceneHandle { set; get; }

        // 加载完成后的调用
        public Action LoadingSuccessCallBack { get; set; }
    }
    
    // 子场景加载上下文
    public interface IUnitySceneContext
    {
        // 开启加载UI界面
        public ETTask OpenLoadingUI(UnityScene unityScene);
        
        // 初始子场景组件
        public ETTask InitComponent(UnityScene unityScene);

        // 开始场景资源加载
        public ETTask StartLoading(UnityScene unityScene);

        // 场景资源加载完成
        public ETTask LoadingCompleted(UnityScene unityScene);

        // 场景关闭
        public ETTask Close(UnityScene unityScene);
    }
    
    public class UnitySceneContext : BaseAttribute
    {
        public UnitySceneEnum UnitySceneEnum;
        
        public UnitySceneContext(UnitySceneEnum UnitySceneEnum)
        {
            this.UnitySceneEnum = UnitySceneEnum;
        }
    }
}