namespace ET.Server;

// 在线玩家秒级监听处理器
[Event(SceneType.Lobby)]
public class GlobalTimeOneSecond_CheckPlayerClockTime: AEvent<Scene, GlobalTimeOneSecond>
{
    protected override async ETTask Run(Scene scene, GlobalTimeOneSecond a)
    {
        var lobbyRoleComponent = scene.GetComponent<LobbyRoleComponent>();
        long now = TimeInfo.Instance.NowSec();
        foreach (var onlineRoleKv in lobbyRoleComponent.OnlineRoles)
        {
            LobbyRole lobbyRole = onlineRoleKv.Value;
            RoleClockComponentHelper.CheckPlayerClockTime(lobbyRole, now);
        }
        await ETTask.CompletedTask;
    }
}