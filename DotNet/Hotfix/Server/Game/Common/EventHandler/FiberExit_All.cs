namespace ET.Server;

[Event(SceneType.All)]
public class FiberExit_All: AEvent<Scene, FiberExit>
{
    protected override async ETTask Run(Scene scene, FiberExit a)
    {
        Log.Info($"服务 : {scene.SceneType} 开始退出....");
        await EventSystem.Instance.PublishAsync(scene, new ExitServerEvent());
        await EventSystem.Instance.PublishAsync(scene, new ExitServerFinishEvent());
        Log.Info($"服务 : {scene.SceneType} 退出完毕!!!");
    }
}