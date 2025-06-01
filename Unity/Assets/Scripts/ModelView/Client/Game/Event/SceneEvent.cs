namespace ET.Client
{
    // 创建 UnityScene 实体后抛出事件
    public struct AfterCreateCurrentUnityScene
    {
        public UnityScene UnityScene;
    }
    
    public struct UnitySceneLoadStart
    {
        public UnityScene UnityScene;
    }
}
