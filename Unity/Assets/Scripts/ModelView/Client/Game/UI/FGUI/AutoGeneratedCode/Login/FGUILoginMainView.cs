//"--This is an automatically generated class by FairyGUIPlugin. Please do not modify it."
using System;
using FairyGUI;

namespace ET.Client
{
	[ComponentOf]
	[FGUITag(FGUIPackage.PKG_Login, FGUIResName.RES_Login_LoginMainView)]
    public class FGUILoginMainView: FGUI, IAwake<GObject>, IDestroy
    {   
        //// 组件变量
		public FGUICommonBG fgui_bg;
		public GTextInput fgui_user_name_titime;
		public GTextInput fgui_loginField;
		public FGUILoginBtn1 fgui_loginBtn;
		public GGroup fgui_login_field_group;

    }
}