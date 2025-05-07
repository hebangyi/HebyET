/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ET.UICommon
{
    public partial class FGUILoginMainView : GComponent
    {
        public GButton fgui_loginBtn;
        public const string URL = "ui://y9rc4gocjrsx5";

        public static FGUILoginMainView CreateInstance()
        {
            return (FGUILoginMainView)UIPackage.CreateObject("UICommon", "LoginMainView");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            fgui_loginBtn = (GButton)GetChild("loginBtn");
        }
    }
}