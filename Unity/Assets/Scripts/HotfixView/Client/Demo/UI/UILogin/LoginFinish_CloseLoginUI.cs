namespace ET.Client
{
	[Event(SceneType.Game)]
	public class LoginFinish_CloseLoginUI: AEvent<Scene, LoginFinish>
	{
		protected override async ETTask Run(Scene scene, LoginFinish args)
		{
			FGUIComponent.Instance.CloseWindow(WindowID.LoginMainView);
		}
	}
}
