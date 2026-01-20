using Spine.Unity;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(UnitEntityGameObjectComponent))]
    [FriendOf(typeof(UnitEntityGameObjectComponent))]
    public static partial class UnitEntityGameObjectComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UnitEntityGameObjectComponent self, GameObject gameObject)
        {
            if (gameObject == null)
            {
                return;
            }
            
            self.GameObject = gameObject;
        }

        [EntitySystem]
        private static void Destroy(this UnitEntityGameObjectComponent self)
        {
            var gameObject = self.GameObject;
            UnityEngine.Object.Destroy(gameObject);
        }
    }
}

