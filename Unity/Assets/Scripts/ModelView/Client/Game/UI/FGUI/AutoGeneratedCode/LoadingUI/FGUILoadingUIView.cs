//"--This is an automatically generated class by FairyGUIPlugin. Please do not modify it."
using System;
using FairyGUI;

namespace ET.Client
{
	[ComponentOf]
	[FGUITag(FGUIPackage.PKG_LoadingUI, FGUIResName.RES_LoadingUI_FGUILoadingUIView)]
    public class FGUILoadingUIView: FGUI, IAwake<GObject>, IDestroy
    {   
        //// 组件变量
		public FGUICommonBG bg;
		public GProgressBar processBar;

    }
}