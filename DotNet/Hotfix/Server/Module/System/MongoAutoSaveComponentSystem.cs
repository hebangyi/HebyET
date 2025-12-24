namespace ET.Server
{
   [FriendOf(typeof(MongoAutoSaveComponent))]
   [EntitySystemOf(typeof(MongoAutoSaveComponent))]
   public static partial class MongoAutoSaveComponentSystem
   {
       
       [EntitySystem]
       private static void Awake(this MongoAutoSaveComponent self)
       {
       }

   }
}