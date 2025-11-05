using UnityEngine;
using YooAsset;

namespace ET.Client
{
    [ChildOf]
    public class UnityScene : Entity, IAwake
    {
        // 场景类型
        public UnitySceneEnum UnitySceneEnum;
        public object[] ParamList;

        // 正在加载场景的句柄
        public SceneHandle LoadingSceneHandle { set; get; }
    }
    
    // 子场景加载上下文
    public interface IUnitySceneContext
    {
        // 开启加载UI界面
        public void OpenLoadingUI(UnityScene unityScene);
        
        // 初始子场景组件
        public void InitComponent(UnityScene unityScene);

        // 开始场景资源加载
        public void StartLoading(UnityScene unityScene);

        // 场景资源加载完成
        public void LoadingFinished(UnityScene unityScene);

        // 场景关闭
        public void Close(UnityScene unityScene);
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