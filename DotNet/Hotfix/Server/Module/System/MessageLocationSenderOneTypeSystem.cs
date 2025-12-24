namespace ET.Server
{
   [FriendOf(typeof(MessageLocationSenderOneType))]
   [EntitySystemOf(typeof(MessageLocationSenderOneType))]
   public static partial class MessageLocationSenderOneTypeSystem
   {
       
       [EntitySystem]
       private static void Awake(this MessageLocationSenderOneType self)
       {
       }

       [EntitySystem]
       private static void Destroy(this MessageLocationSenderOneType self)
       {
       }

   }
}