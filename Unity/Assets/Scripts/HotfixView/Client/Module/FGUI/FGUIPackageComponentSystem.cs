using System.Collections.Generic;

namespace ET.Client
{
    [EntitySystemOf(typeof(FGUIPackageComponent))]
    [FriendOf(typeof(FGUIPackageComponent))]
    public static partial class FGUIPackageComponentSystem
    {
        [EntitySystem]
        private static void Awake(this FGUIPackageComponent self)
        {
            self.Awake();
        }
    }
}