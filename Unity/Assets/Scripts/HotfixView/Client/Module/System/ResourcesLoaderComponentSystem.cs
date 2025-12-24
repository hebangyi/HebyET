namespace ET.Client
{
   [FriendOf(typeof(ResourcesLoaderComponent))]
   [EntitySystemOf(typeof(ResourcesLoaderComponent))]
   public static partial class ResourcesLoaderComponentSystem
   {
       
       [EntitySystem]
       private static void Awake(this ResourcesLoaderComponent self)
       {
       }

       [EntitySystem]
       private static void Awake(this ResourcesLoaderComponent self, string p1)
       {
       }

       [EntitySystem]
       private static void Destroy(this ResourcesLoaderComponent self)
       {
       }

   }
}