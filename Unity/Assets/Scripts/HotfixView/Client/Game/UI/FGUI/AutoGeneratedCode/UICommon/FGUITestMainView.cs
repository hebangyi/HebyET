/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ET.UICommon
{
    public partial class FGUITestMainView : GComponent
    {
        public GButton fgui_Test;
        public const string URL = "ui://y9rc4gock1qk3";

        public static FGUITestMainView CreateInstance()
        {
            return (FGUITestMainView)UIPackage.CreateObject("UICommon", "TestMainView");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            fgui_Test = (GButton)GetChild("Test");
        }
    }
}