using System;
using System.Collections.Generic;
using System.Reflection;
using FairyGUI;

namespace ET.Client
{
    [ComponentOf]
    public class FGUIComponent : Entity, IAwake
    {
        public static FGUIComponent Instance;

        public Dictionary<int, UIBaseWindow> AllWindowsDict = new ();
        public Dictionary<int, UIBaseWindow> VisibleWindowsDict = new ();    // 显示的WindowView
        public Dictionary<int, UIBaseWindow> InVisibleWindowsDict = new ();  // 隐藏的WindowView

        public Dictionary<UIWindowType, FGUILayer> AllWindowTypes = new ();
    }

    public class ShowWindowData
    {
        public object[] args;
    }
}