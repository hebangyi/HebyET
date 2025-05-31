namespace ET.Client
{
	[Event(SceneType.Game)]
	public class LoginFinish_CreateLobbyUI: AEvent<Scene, LoginFinish>
	{
		protected override async ETTask Run(Scene scene, LoginFinish args)
		{
			await SceneChangeHelper.SceneChangeTo(scene, UnitySceneType.Test);
			
			// 打开加载界面
			await FGUIComponent.Instance.ShowWindowAsync(WindowID.LoadingUIView);
			// 关闭登录界面
			FGUIComponent.Instance.CloseWindow(WindowID.LoginMainView);
		}
	}
}
