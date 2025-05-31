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
}