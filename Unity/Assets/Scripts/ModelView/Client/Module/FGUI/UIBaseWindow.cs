using System;
using System.Reflection;
using FairyGUI;

namespace ET.Client
{
    [ChildOf]
    [EnableMethod]
    public class UIBaseWindow : Entity, IAwake, IDestroy
    {
        public WindowID WindowId = WindowID.None;
        // 根节点的GObject
        public GObject GObject;
        // UI 类型
        public UIWindowType WindowType = UIWindowType.Normal;
    }
}