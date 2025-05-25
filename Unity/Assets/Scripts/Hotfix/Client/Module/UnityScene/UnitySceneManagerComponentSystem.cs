using ET.Client;
using ET.Model;

namespace ET.Hotfix
{
    [EntitySystemOf(typeof(UnitySceneManagerComponent))]
    public static partial class UnitySceneManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UnitySceneManagerComponent self)
        {
            UnitySceneManagerComponent.Instance = self;
        }
    }
}

