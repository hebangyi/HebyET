namespace ET.Client
{
   [FriendOf(typeof(UnitySceneClientWorldManagerComponent))]
   [EntitySystemOf(typeof(UnitySceneClientWorldManagerComponent))]
   public static partial class UnitySceneClientWorldManagerComponentSystem
   {
       
       [EntitySystem]
       private static void Awake(this UnitySceneClientWorldManagerComponent self)
       {
       }

       [EntitySystem]
       private static void Destroy(this UnitySceneClientWorldManagerComponent self)
       {
       }

   }
}