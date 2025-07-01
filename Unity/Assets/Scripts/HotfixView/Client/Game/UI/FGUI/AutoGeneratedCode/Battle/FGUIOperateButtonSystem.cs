//"--This is an automatically generated class by FairyGUIPlugin. Please do not modify it."
using FairyGUI;


namespace ET.Client
{
    [EntitySystemOf(typeof(FGUIOperateButton))]
    public static partial class FGUIOperateButtonSystem
    {
        [EntitySystem]
        public static void Awake(this FGUIOperateButton self, FairyGUI.GObject go)
        {
			self.UIPackageName = "Battle";
	        self.UIResourceName = "FGUIOperateButton";
	        self.UIResURL = "ui://Battle/FGUIOperateButton";
	        self.FUIName = "FGUIOperateButton";
			self.GObject = go;
            var com = go.asCom;
			self.TouchArea = (GGraph)com.GetChild("TouchArea");
			self.YaoGanBg3 = (GImage)com.GetChild("YaoGanBg3");
			self.YaoGanBg2 = (GImage)com.GetChild("YaoGanBg2");
			self.YaoGanBg1 = (GImage)com.GetChild("YaoGanBg1");
			self.YaoGanImg = (GImage)com.GetChild("YaoGanImg");
			self.ButtonGroup = (GGroup)com.GetChild("ButtonGroup");

        }
        
        
        [EntitySystem]
        public static void Destroy(this FGUIOperateButton self)
        {
			self.TouchArea = null;
			self.YaoGanBg3 = null;
			self.YaoGanBg2 = null;
			self.YaoGanBg1 = null;
			self.YaoGanImg = null;
			self.ButtonGroup = null;

        }
    }
}

