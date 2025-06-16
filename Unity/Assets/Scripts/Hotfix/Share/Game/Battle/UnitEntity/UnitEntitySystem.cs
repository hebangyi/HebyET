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
# if DOTNET
            self.DirtySyncUnitEntityData = ObjectPool.Instance.Fetch<Dictionary<ushort, IUnitEntityElemData>>();
# endif
        }

        [EntitySystem]
        private static void Destroy(this UnitEntity self)
        {
            ObjectPool objectPool = ObjectPool.Instance;
            // 回收所有的EntityData
            foreach (var dataElement in self.UnitEntityData.Values)
            {
                if (dataElement is MessageObject messageObject)
                {
                    messageObject.Dispose();
                }
            }
            
            self.UnitEntityData.Clear();
            objectPool.Recycle(self.UnitEntityData);
            self.UnitEntityData = null;

            var world = self.GetParent<World>();
            if (world != null)
            {
                world.RemoveEntity(self);
            }
            
            
# if DOTNET

            self.DirtySyncUnitEntityData.Clear();
            objectPool.Recycle(self.DirtySyncUnitEntityData);
            self.DirtySyncUnitEntityData = null;
# endif
        }


        // 是否有 ElementData 数据
        public static bool HasUnitEntityElementData<T>(this UnitEntity self) where T: IUnitEntityElemData
        {
            Type type = typeof(T);
            var componentId = OpcodeType.Instance.GetOpcode(type);
            return self.UnitEntityData.ContainsKey(componentId);
        }


        public static T GetOrCreateUnitEntityElemData<T>(this UnitEntity self, bool addDirty = true) where T : IUnitEntityElemData
        {
            Type type = typeof(T);
            var componentId = OpcodeType.Instance.GetOpcode(type);
            var elemData = self.UnitEntityData.GetValueOrDefault(componentId);
            if (elemData != null)
            {
                return (T)elemData;
            }
            
            var obj = type.GetMethod("Create")?.Invoke(null, null);
            T instance = (T)obj;
            self.UnitEntityData[componentId] = instance;

            # if DOTNET
            if (addDirty)
            {
                self.DirtySyncUnitEntityData[componentId] = instance;
            }
            # endif
            return instance;
        }
    }
}
