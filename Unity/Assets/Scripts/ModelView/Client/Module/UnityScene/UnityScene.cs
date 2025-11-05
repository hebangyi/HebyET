using UnityEngine;
using YooAsset;

namespace ET.Client
{
    [ChildOf]
    public class UnityScene : Entity, IAwake
    {
        // 场景类型
        public UnitySceneType UnitySceneType;
        public object[] ParamList;

        // 加载场景的句柄
        public SceneHandle SceneHandle { set; get; }
    }


    public interface IUnitySceneContext
    {
        public void InitComponent();

        public void StartLoading();

        public void LoadingFinished();
    }
}