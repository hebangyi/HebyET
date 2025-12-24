namespace ET.Client
{
    [EntitySystemOf(typeof(UnitEntityGameObjectComponent))]
    [FriendOf(typeof(UnitEntityGameObjectComponent))]
    public static partial class UnitEntityGameObjectComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UnitEntityGameObjectComponent self)
        {

        }
        
        [EntitySystem]
        private static void Destroy(this UnitEntityGameObjectComponent self)
        {
            var gameObject = self.GameObject;
            UnityEngine.Object.Destroy(gameObject);
        }
    }
}

