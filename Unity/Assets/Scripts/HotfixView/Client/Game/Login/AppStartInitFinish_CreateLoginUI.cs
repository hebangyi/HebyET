using System;
using FairyGUI;

namespace ET.Client
{
    [Event(SceneType.Game)]
    public class AppStartInitFinish_CreateLoginUI: AEvent<Scene, AppStartInitFinish>
    {
        protected override async ETTask Run(Scene root, AppStartInitFinish args)
        {
            // await UIHelper.Create(root, UIType.UILogin, UILayer.Mid);
            // 加载常规的UI包
            await FGUIPackageComponent.Instance.TryAddPackageAsync("Example");
            await FGUIPackageComponent.Instance.TryAddPackageAsync(FGUIPackage.PKG_UICommon);
			
            // await FGUIComponent.Instance.ShowWindowAsync(WindowID.FGUIBattleOperationMainView);
            await FGUIComponent.Instance.ShowWindowAsync(WindowID.FGUILoginMainView);
            // await SceneChangeHelper.SceneChangeTo(root, UnitySceneType.);
        }
    }
}