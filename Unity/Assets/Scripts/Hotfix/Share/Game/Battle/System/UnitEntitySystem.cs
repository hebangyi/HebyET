using System;
using System.Collections.Generic;

namespace ET
{
    [EntitySystemOf(typeof(UnitEntity))]
    [FriendOf(typeof(UnitEntity))]
    public static partial class UnitEntitySystem
    {
        [EntitySystem]
        private static void Awake(this UnitEntity self)
        {
            self.UnitEntityData = ObjectPool.Instance.Fetch<Dictionary<ushort, IUnitEntityElemData>>();
            self.UnitEntityLogicData = ObjectPool.Instance.Fetch<Dictionary<Type, IUnitEntityLogicElemData>>();
        }

        [EntitySystem]
        private static void Destroy(this UnitEntity self)
        {
            var world = self.GetParent<World>();
            if (world != null)
            {
                world.RemoveEntity(self);
            }
            
            // 回收所有的EntityData
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
            
            
            self.UnitEntityLogicData.Clear();
            objectPool.Recycle(self.UnitEntityLogicData);
            self.UnitEntityLogicData = null;
            
            self.Dispose();
        }
    }
}