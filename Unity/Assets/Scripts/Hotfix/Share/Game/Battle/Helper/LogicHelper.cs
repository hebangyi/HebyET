using System;
using System.Collections.Generic;

namespace ET
{
    public static class LogicHelper
    {
        public static void Tick(this World self)
        {
            self.Frame++;
            // Log.Info($"World Id : {self.Id} Tick Frame: {self.Frame}");
            foreach (var comId2LogicsKv in BattleUnitEntityLogicManagerComponent.Instance.Type2TickLogics)
            {
                var logicHandler = comId2LogicsKv.Value;
                logicHandler.OnTick(self);
            }
            
            self.SyncHandler?.Sync();
        }
    
        public static T CreateUnitEntityElemData<T>(this UnitEntity self) where T : IUnitEntityElemData
        {
            var world = self.GetParent<World>();
        
            Type type = typeof(T);
            var componentId = OpcodeType.Instance.GetOpcode(type);
            if (self.UnitEntityData.ContainsKey(componentId))
            {
                Log.Error("this world component id is already exist, can not create entity element data");
                return default;
            }

            var methodInfo = type.GetMethod("Create");
            var obj = methodInfo.Invoke(null, new object[]{self.InsId, world.DirtyHandler ,true});
            T instance = (T)obj;
            self.UnitEntityData[componentId] = instance;
            return instance;
        }

        public static T CreateUnitEntityLogicElemData<T>(this UnitEntity self) where T : class, IUnitEntityLogicElemData
        {
            Type type = typeof(T);
            if (self.UnitEntityLogicData.ContainsKey(type))
            {
                Log.Error("this world component id is already exist, can not create entity element data");
                return default;
            }

            var logicElemData = Activator.CreateInstance(type) as IUnitEntityLogicElemData;
            if (logicElemData == null)
            {
                return null;
            }
            
            self.UnitEntityLogicData[type] = logicElemData;
            return (T)logicElemData;
        }
        
        
        public static T GetUnitEntityLogicElemData<T>(this UnitEntity self) where T : class, IUnitEntityLogicElemData
        {
            Type type = typeof(T);
            T elemData = self.UnitEntityLogicData.GetValueOrDefault(type) as T;
            return elemData;
        }
        
    }
}
