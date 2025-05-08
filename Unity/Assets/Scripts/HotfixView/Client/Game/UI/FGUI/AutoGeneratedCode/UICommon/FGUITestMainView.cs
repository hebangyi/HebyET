//"--This is an automatically generated class by FairyGUIPlugin. Please do not modify it."
using ETModel;
using FairyGUI;

namespace ET.Client
{
    public class FGUITestMainView: Entity, IAwake<GObject>, IDestroy
    {
        public String UIPackageName = "UICommon";
        public String UIResourceName = "TestMainView";
        public String UIResURL = "ui://UICommon/TestMainView";
        public String FUIName = "UICommon_TestMainView";
        public System.Action<FGUITestMainView> OnPreDisposeEvent;
//// 组件变量
		public GButton fgui_Test;


    }
}