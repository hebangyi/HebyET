/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ET.Login
{
    public partial class FGUILoginMainView : GComponent
    {
        public GButton fgui_loginBtn;
        public GTextInput fgui_loginField;
        public const string URL = "ui://yn21ng5jcirq0";

        public static FGUILoginMainView CreateInstance()
        {
            return (FGUILoginMainView)UIPackage.CreateObject("Login", "LoginMainView");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            fgui_loginBtn = (GButton)GetChild("loginBtn");
            fgui_loginField = (GTextInput)GetChild("loginField");
        }
    }
}