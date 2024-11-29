namespace ET.Server;

[Event(SceneType.Main)]
public class InitServerFinish1_Etcd : AEvent<Scene, InitServerFinish1>
{
    protected override async ETTask Run(Scene scene, InitServerFinish1 a)
    {
        Log.Info("开始注册ETCD 并监听ETCD");
        var etcdComponent = scene.GetComponent<EtcdComponent>();
        etcdComponent.StartRegAndWatch();
        await ETTask.CompletedTask;
    }
}