namespace ET.Server;

[Event(SceneType.Lobby)]
public class RoleClockComponent_LobbyRoleInit : AEvent<Scene, LobbyRoleDBInitEvent>
{
    protected override async ETTask Run(Scene scene, LobbyRoleDBInitEvent args)
    {
        var lobbyRole = args.LobbyRole;
        RoleClockComponentHelper.InitData(lobbyRole);
        await ETTask.CompletedTask;
    }
}

[FriendOf(typeof(RoleClockComponent))]
public static class RoleClockComponentHelper
{
    public static void InitData(LobbyRole lobbyRole)
    {
        var roleCommonComponent = lobbyRole.GetComponent<RoleClockComponent>();
        if (roleCommonComponent.RoleClockData == null)
        {
            roleCommonComponent.RoleClockData = new RoleClockServerData();
        }
    }

    public static void CheckPlayerClockTime(LobbyRole lobbyRole)
    {
        long now = TimeInfo.Instance.ServerNowSec();

        var roleCommonComponent = lobbyRole.GetComponent<RoleClockComponent>();
        var roleClockData = roleCommonComponent.RoleClockData;

        var lastDayUpdateTime = roleClockData.LastDayUpdateTime;
        var lastWeekUpdateTime = roleClockData.LastWeekUpdateTime;
        var lastMonthUpdateTime = roleClockData.LastMonthUpdateTime;

        if (TimeHelper.IsCrossDay(lastDayUpdateTime, now))
        {
            EventSystem.Instance.Publish(lobbyRole.Root(), new LobbyRoleCrossDay1Event { LobbyRole = lobbyRole });
            EventSystem.Instance.Publish(lobbyRole.Root(), new LobbyRoleCrossDay2Event { LobbyRole = lobbyRole });
            EventSystem.Instance.Publish(lobbyRole.Root(), new LobbyRoleCrossDay3Event { LobbyRole = lobbyRole });

            roleClockData.LastDayUpdateTime = now;
        }

        if (TimeHelper.IsCrossWeek(lastWeekUpdateTime, now))
        {
            EventSystem.Instance.Publish(lobbyRole.Root(), new LobbyRoleCrossWeek1Event { LobbyRole = lobbyRole });
            EventSystem.Instance.Publish(lobbyRole.Root(), new LobbyRoleCrossWeek2Event { LobbyRole = lobbyRole });
            EventSystem.Instance.Publish(lobbyRole.Root(), new LobbyRoleCrossWeek3Event { LobbyRole = lobbyRole });

            roleClockData.LastWeekUpdateTime = now;
        }

        if (TimeHelper.IsCrossMonth(lastMonthUpdateTime, now))
        {
            EventSystem.Instance.Publish(lobbyRole.Root(), new LobbyRoleCrossMonth1Event { LobbyRole = lobbyRole });
            EventSystem.Instance.Publish(lobbyRole.Root(), new LobbyRoleCrossMonth2Event { LobbyRole = lobbyRole });
            EventSystem.Instance.Publish(lobbyRole.Root(), new LobbyRoleCrossMonth3Event { LobbyRole = lobbyRole });

            roleClockData.LastMonthUpdateTime = now;
        }
    }
}