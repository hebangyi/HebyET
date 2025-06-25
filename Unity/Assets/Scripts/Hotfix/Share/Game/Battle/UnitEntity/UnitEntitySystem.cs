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
        }

        [EntitySystem]
        private static void Destroy(this UnitEntity self)
        {
            foreach (var unitEntityElemDataKv in self.UnitEntityData)
            {
                var elemId = unitEntityElemDataKv.Key;
                var logics = BattleUnitEntityDataLogicManagerComponent.Instance.GetInitLogicByComponentId(elemId);
                if (logics != null)
                {
                    foreach (var logic in logics)
                    {
                        logic.OnDestroy(self);
                    }
                }
            }

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
                world.DestroyEntity(self);
            }
            self.Dispose();
        }

        // 是否有 ElementData 数据
        public static bool HasUnitEntityElementData<T>(this UnitEntity self) where T : IUnitEntityElemData
        {
            Type type = typeof(T);
            var componentId = OpcodeType.Instance.GetOpcode(type);
            return self.UnitEntityData.ContainsKey(componentId);
        }

        public static T GetUnitEntityElemData<T>(this UnitEntity self) where T : class
        {
            Type type = typeof(T);
            var componentId = OpcodeType.Instance.GetOpcode(type);
            T elemData = self.UnitEntityData.GetValueOrDefault(componentId) as T;
            return elemData;
        }

        // 创建UnitEntity时使用
        public static T GetOrCreateUnitEntityElemData<T>(this UnitEntity self, bool addDirty = true) where T : IUnitEntityElemData
        {
            Type type = typeof(T);
            var componentId = OpcodeType.Instance.GetOpcode(type);
            var elemData = self.UnitEntityData.GetValueOrDefault(componentId);
            if (elemData != null)
            {
                return (T)elemData;
            }

            var methodInfo = type.GetMethod("Create");
            var obj = methodInfo.Invoke(null, new object[]{true});
            T instance = (T)obj;
            self.UnitEntityData[componentId] = instance;
            
            var logics = BattleUnitEntityDataLogicManagerComponent.Instance.GetInitLogicByComponentId(componentId);
            if (logics != null)
            {
                foreach (var logic in logics)
                {
                    logic.OnInit(self);
                }
            }
            
            return instance;
        }
    }
}