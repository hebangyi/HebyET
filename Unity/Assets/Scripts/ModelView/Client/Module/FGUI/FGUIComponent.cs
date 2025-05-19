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
        public Dictionary<int, UIBaseWindow> VisibleWindowsDic = new ();

        public Dictionary<UIWindowType, FGUILayer> AllWindowTypes = new ();
    }

    public class ShowWindowData
    {
        public object[] args;
    }
}