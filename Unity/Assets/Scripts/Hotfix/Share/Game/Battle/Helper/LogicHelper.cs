using System;
using System.Collections.Generic;

namespace ET
{
    public static class LogicHelper
    {
        public static void Tick(this World self)
        {
            var comId2Logics = BattleUnitEntityLogicManagerComponent.Instance.CompId2TickLogics;
            foreach (var comId2LogicsKv in comId2Logics)
            {
                var logicHandlers = comId2LogicsKv.Value;
                foreach (var logicHandler in logicHandlers)
                {
                    logicHandler.OnTick();
                }
            }
        }
    
        public static T GetOrCreateUnitEntityElemData<T>(this UnitEntity self, bool addDirty = true) where T : IUnitEntityElemData
        {
            var world = self.GetParent<World>();
            if (world.WorldMode != WorldMode.Logic)
            {
                Log.Error("this world is not server , can not create entity element data");
                return default;
            }
        
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

            if (world.AllEntity.ContainsKey(self.InsId))
            {
                world.PublishEvent(new CreateUnitEntityElementData(){UnitEntity = self, UnitEntityElemData = instance, ComponentId = componentId});    
            }
            return instance;
        }
    }
}
