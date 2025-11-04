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
                if (handler is IClientEleInit viewInitLogic)
                {
                    var componentId = viewInitLogic.WatchComponentId();
                    var logics = self.CompId2InitViewLogics.GetValueOrDefault(componentId);
                    if (logics == null)
                    {
                        logics = new List<IClientEleInit>();
                        self.CompId2InitViewLogics.Add(componentId, logics);
                    }

                    logics.Add(viewInitLogic);
                }

                if (handler is IClientEleUpdate dataUpdateLogic)
                {
                    var componentId = dataUpdateLogic.WatchComponentId();

                    var logics = self.CompId2ElementDataUpdates.GetValueOrDefault(componentId);
                    if (logics == null)
                    {
                        logics = new List<IClientEleUpdate>();
                        self.CompId2ElementDataUpdates.Add(componentId, logics);
                    }

                    logics.Add(dataUpdateLogic);
                }
            }
            
            var cycleAttributeTypes = CodeTypes.Instance.GetAttributeTypes(typeof(ClientLifeCycleAttribute));
            foreach (var type in cycleAttributeTypes)
            {
                var cycleUnitEntity = Activator.CreateInstance(type);
                if (cycleUnitEntity is IClientLifeCycle clientLifeCycle)
                {
                    var clientLifeCycleAttribute = type.GetCustomAttribute(typeof(ClientLifeCycleAttribute)) as ClientLifeCycleAttribute;
                    var ueTypeEnum = clientLifeCycleAttribute.UeTypeEnum;
                    self.UnitEntityLifeCycles[ueTypeEnum] = clientLifeCycle;
                }
            }
        }

        public static List<IClientEleInit> GetInitViewLogicByComponentId(this ClientWorldLogicManagerComponent self,
        ushort componentId)
        {
            return self.CompId2InitViewLogics.GetValueOrDefault(componentId);
        }

        public static List<IClientEleUpdate> GetUpdateLogicByComponentId(this ClientWorldLogicManagerComponent self,
        ushort componentId)
        {
            return self.CompId2ElementDataUpdates.GetValueOrDefault(componentId);
        }
    }
}