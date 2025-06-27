namespace ET.Server
{
	[ComponentOf(typeof(Session))]
	public class SessionLobbyPlayerComponent : Entity, IAwake<long>, IDestroy
	{
		public long RoleId;
	}
}