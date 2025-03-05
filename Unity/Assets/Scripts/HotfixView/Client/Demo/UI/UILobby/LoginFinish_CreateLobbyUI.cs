namespace ET.Client
{
	[Event(SceneType.Game)]
	public class LoginFinish_CreateLobbyUI: AEvent<Scene, LoginFinish>
	{
		protected override async ETTask Run(Scene scene, LoginFinish args)
		{
			await UIHelper.Create(scene, UIType.UILobby, UILayer.Mid);
		}
	}
}
