using System.Collections.Generic;

namespace ET.Client
{
   [FriendOf(typeof(ClientUnitEntity))]
   [EntitySystemOf(typeof(ClientUnitEntity))]
   public static partial class ClientUnitEntitySystem
   {
       
       [EntitySystem]
       private static void Awake(this ClientUnitEntity self)
       {
           self.UnitEntityData = ObjectPool.Instance.Fetch<Dictionary<ushort, IUnitEntityElemData>>();
       }

       [EntitySystem]
       private static void Destroy(this ClientUnitEntity self)
       {
           var world = self.GetParent<ClientWorld>();
           if (world != null)
           {
               world.RemoveEntity(self);
           }
           
           foreach (var dataElement in self.UnitEntityData.Values)
           {
               if (dataElement is MessageObject messageObject)
               {
                   messageObject.Dispose();
               }
           }
           
           ObjectPool objectPool = ObjectPool.Instance;
           self.UnitEntityData.Clear();
           objectPool.Recycle(self.UnitEntityData);
           self.UnitEntityData = null;
       }
   }
}