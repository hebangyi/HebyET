using FairyGUI;

namespace ET.Client
{
    [EntitySystemOf(typeof(FGUILayer))]
    [FriendOf(typeof(FGUILayer))]
    public static partial class FGUILayerSystem
    {
        [EntitySystem]
        private static void Awake(this FGUILayer self, GObject gObject)
        {
            self.GObject = gObject;
        }
    }
}
