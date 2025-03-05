namespace ET.Server;

public static class LobbyMessageHelper
{
    public static void SendToClient(this LobbyRole lobbyRole, IMessage message)
    {
        lobbyRole.Root().GetComponent<MessageLocationSenderComponent>().Get(LocationType.GateSession).Send(lobbyRole.Id, message);
    }
}