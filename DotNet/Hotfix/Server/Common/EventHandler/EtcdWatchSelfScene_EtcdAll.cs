namespace ET.Server;

[Event(SceneType.All)]
[FriendOf(typeof(EtcdClientComponent))]
public class EtcdWatchSelfScene_EtcdAll : AEvent<Scene, EtcdWatchSelfSceneEvent>
{
    protected override async ETTask Run(Scene scene, EtcdWatchSelfSceneEvent a)
    {
        var etcdClientComponent = scene.GetComponent<EtcdClientComponent>();
        etcdClientComponent.IsRegisterOver = true;
        await ETTask.CompletedTask;
    }
}