namespace ET.Server
{
   [FriendOf(typeof(MongoDBComponent))]
   [EntitySystemOf(typeof(MongoDBComponent))]
   public static partial class MongoDBComponentSystem
   {
       
       [EntitySystem]
       private static void Awake(this MongoDBComponent self)
       {
       }

   }
}