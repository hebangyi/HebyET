//"--This is an automatically generated class by FairyGUIPlugin. Please do not modify it."
using System;
using FairyGUI;

namespace ET.Client
{
    public class FGUILoginMainView: Entity, IAwake<GObject>, IDestroy
    {
        public const String UIPackageName = "Login";
        public const String UIResourceName = "LoginMainView";
        public const String UIResURL = "ui://Login/LoginMainView";
        public const String FUIName = "Login_LoginMainView";
        
        //// 组件变量
		public GTextInput fgui_testName;
		public FGUILoginBtn fgui_loginBtn;
		public GTextInput fgui_loginField;
		public GComponent fgui_headIcon;


    }
}