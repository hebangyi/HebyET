//"--This is an automatically generated class by FairyGUIPlugin. Please do not modify it."
using System;
using FairyGUI;

namespace ET.Client
{
	[ComponentOf]
	[FGUITag(FGUIPackage.PKG_Login, FGUIResName.RES_Login_FGUILoginMainView)]
    public class FGUILoginMainView: FGUI, IAwake<GObject>, IDestroy
    {   
        //// 组件变量
		public FGUICommonBG bg;
		public GTextInput loginUserName;
		public GGraph loginFieldBg;
		public GTextInput loginField;
		public GButton loginBtn;
		public GTextField loginBtbTitile;
		public GGroup loginFieldGroup;

    }
}