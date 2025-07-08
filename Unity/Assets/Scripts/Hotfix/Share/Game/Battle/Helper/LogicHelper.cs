using System;
using System.Collections.Generic;

namespace ET
{
    public static class LogicHelper
    {
        public static void Tick(this World self)
        {
            self.Frame++;
            Log.Info($"World Id : {self.Id} Tick Frame: {self.Frame}");
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
            if (world.WorldMode != WorldMode.Logic)
            {
                Log.Error("this world is not server , can not create entity element data");
                return default;
            }
        
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
    }
}
