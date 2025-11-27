using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ET
{
    [EntitySystemOf(typeof(LogicWorldLogicManagerComponent))]
    [FriendOf(typeof(LogicWorldLogicManagerComponent))]
    public static partial class LogicWorldLogicManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LogicWorldLogicManagerComponent self)
        {
            LogicWorldLogicManagerComponent.Instance = self;

            var logicTypes = CodeTypes.Instance.GetAttributeTypes(typeof(UnitEntityLogicAttribute));
            foreach (var type in logicTypes)
            {
                var handler = Activator.CreateInstance(type);
                if (handler is ILogicEleInit dataLogic)
                {
                    var componentId = dataLogic.WatchComponentId();

                    var logics = self.CompId2InitLogics.GetValueOrDefault(componentId);
                    if (logics == null)
                    {
                        logics = new List<ILogicEleInit>();
                        self.CompId2InitLogics.Add(componentId, logics);
                    }

                    logics.Add(dataLogic);
                }

                if (handler is ILogicTickUpdate tickLogic)
                {
                    self.Type2TickLogics[handler.GetType()] = tickLogic;
                }

                if (handler is ILogicClientInput clientInput)
                {
                    var componentId = clientInput.WatchComponentId();
                    self.ClientInputLogics[componentId] = clientInput;
                }
            }
            
            
            var contexts = CodeTypes.Instance.GetAttributeTypes(typeof(LogicUnitEntityContext));
            foreach (var type in contexts)
            {
                var cycleUnitEntity = Activator.CreateInstance(type);
                if (cycleUnitEntity is ILogicUnitEntityContext context)
                {
                    var clientLifeCycleAttribute = type.GetCustomAttribute(typeof(LogicUnitEntityContext)) as LogicUnitEntityContext;
                    var ueTypeEnum = clientLifeCycleAttribute.UeTypeEnum;
                    self.UnitEntityContexts[ueTypeEnum] = context;
                }
            }
        }

        public static List<ILogicEleInit> GetInitLogicByComponentId(this LogicWorldLogicManagerComponent self, ushort componentId)
        {
            return self.CompId2InitLogics.GetValueOrDefault(componentId);
        }

        public static ILogicTickUpdate GetTickLogicByType(this LogicWorldLogicManagerComponent self, Type logicType)
        {
            return self.Type2TickLogics.GetValueOrDefault(logicType);
        }


        public static ILogicClientInput GetClientInputByComponentId(this LogicWorldLogicManagerComponent self, ushort componentId)
        {
            return self.ClientInputLogics.GetValueOrDefault(componentId);
        }
    }
}