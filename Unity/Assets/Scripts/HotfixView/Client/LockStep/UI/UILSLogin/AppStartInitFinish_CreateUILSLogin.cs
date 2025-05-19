namespace ET.Client
{
	[Event(SceneType.LockStep)]
	public class AppStartInitFinish_CreateUILSLogin: AEvent<Scene, AppStartInitFinish>
	{
		protected override async ETTask Run(Scene root, AppStartInitFinish args)
		{
			// await UIHelper.Create(root, UIType.UILSLogin, UILayer.Mid);
			// 使用 FairyGUI 创建UI
			// var fguiComponent = root.GetComponent<FGUIComponent>();
		}
	}
}
