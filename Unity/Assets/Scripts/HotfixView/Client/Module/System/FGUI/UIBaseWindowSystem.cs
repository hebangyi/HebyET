using System.Collections.Generic;
using System.Transactions;
using FairyGUI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIBaseWindow))]
    [FriendOf(typeof(UIBaseWindow))]
    public static partial class UIBaseWindowSystem
    {
        [EntitySystem]
        private static void Awake(this UIBaseWindow self, GObject gObject, WindowID windowID)
        {
            self.GObject = gObject;
            self.WindowId = windowID;
        }
        
        [EntitySystem]
        private static void Destroy(this UIBaseWindow self)
        {
            var fguiLayer = FGUIComponent.Instance.AllWindowTypes.GetValueOrDefault(self.WindowType);
            if (fguiLayer != null)
            {
                fguiLayer.RemoveWindow(self.GObject);
            }
            self.GObject.Dispose();
            self.GObject = null;
        }
    }
}

