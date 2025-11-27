using System;
using System.Collections.Generic;
using System.Reflection;

namespace ET.Client
{
    [EntitySystemOf(typeof(ClientWorldLogicManagerComponent))]
    [FriendOf(typeof(ClientWorldLogicManagerComponent))]
    public static partial class ClientWorldLogicManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ClientWorldLogicManagerComponent self)
        {
            ClientWorldLogicManagerComponent.Instance = self;

            var viewTypes = CodeTypes.Instance.GetAttributeTypes(typeof(UnitEntityViewLogicAttribute));
            foreach (var type in viewTypes)
            {
                var handler = Activator.CreateInstance(type);
                if (handler is IClientElemEleInit viewInitLogic)
                {
                    var componentId = viewInitLogic.WatchComponentId();
                    var logics = self.CompId2InitViewLogics.GetValueOrDefault(componentId);
                    if (logics == null)
                    {
                        logics = new List<IClientElemEleInit>();
                        self.CompId2InitViewLogics.Add(componentId, logics);
                    }

                    logics.Add(viewInitLogic);
                }

                if (handler is IClientElemEleUpdate dataUpdateLogic)
                {
                    var componentId = dataUpdateLogic.WatchComponentId();

                    var logics = self.CompId2ElementDataUpdates.GetValueOrDefault(componentId);
                    if (logics == null)
                    {
                        logics = new List<IClientElemEleUpdate>();
                        self.CompId2ElementDataUpdates.Add(componentId, logics);
                    }

                    logics.Add(dataUpdateLogic);
                }
            }
            
            var cycleAttributeTypes = CodeTypes.Instance.GetAttributeTypes(typeof(ClientUnitEntityContext));
            foreach (var type in cycleAttributeTypes)
            {
                var cycleUnitEntity = Activator.CreateInstance(type);
                if (cycleUnitEntity is IClientUnitEntityContext clientLifeCycle)
                {
                    var clientLifeCycleAttribute = type.GetCustomAttribute(typeof(ClientUnitEntityContext)) as ClientUnitEntityContext;
                    var ueTypeEnum = clientLifeCycleAttribute.UeTypeEnum;
                    self.UnitEntityContexts[ueTypeEnum] = clientLifeCycle;
                }
            }
        }

        public static List<IClientElemEleInit> GetInitViewLogic(this ClientWorldLogicManagerComponent self,
        ushort componentId)
        {
            return self.CompId2InitViewLogics.GetValueOrDefault(componentId);
        }

        public static List<IClientElemEleUpdate> GetUpdateLogic(this ClientWorldLogicManagerComponent self,
        ushort componentId)
        {
            return self.CompId2ElementDataUpdates.GetValueOrDefault(componentId);
        }

        public static IClientUnitEntityContext GetClientUnitEntityContext(this ClientWorldLogicManagerComponent self,
        UETypeEnum ueTypeEnum)
        {
            return self.UnitEntityContexts.GetValueOrDefault(ueTypeEnum);
        }
    }
}