namespace ET.Server;

// 秒级定时处理器
public struct LobbyRoleOneSecEvent
{
    public LobbyRole LobbyRole;
}

// 注意 LobbyRoleCrossDay 和 GlobalTimeCrossOneDay 不能区分先后顺序
public struct LobbyRoleCrossDay1Event
{
    public LobbyRole LobbyRole;
}

public struct LobbyRoleCrossDay2Event
{
    public LobbyRole LobbyRole;
}

public struct LobbyRoleCrossDay3Event
{
    public LobbyRole LobbyRole;
}